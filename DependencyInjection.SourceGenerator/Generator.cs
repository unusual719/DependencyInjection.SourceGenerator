namespace DependencyInjection.SourceGenerator;

[Generator(LanguageNames.CSharp)]
public class Generator : IIncrementalGenerator
{
    private static readonly string _fileName = @"ServiceCollectionExtensions.g.cs";
    private static readonly string _className = "ServiceCollectionExtensions_G";

    private static void Execute(Compilation compilation, ImmutableArray<DependencyInjectionDescriptor> autoRegisteredClass, SourceProductionContext context)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("        // Services");

        foreach (var item in autoRegisteredClass)
        {
            if (item.ServiceDescriptor != null)
            {
                var descriptor = item.ServiceDescriptor;
                if (descriptor.ServiceTypes.Count == 1)
                {
                    var serviceType = descriptor.ServiceTypes.First();
                    builder.AppendLine($"        services.TryAddEnumerable(ServiceDescriptor.Describe(typeof({serviceType}), typeof({descriptor.DeclaredType}), ServiceLifetime.{descriptor.Lifetime}));");
                }
                else
                {
                    builder.AppendLine($"        services.TryAddEnumerable(ServiceDescriptor.Describe(typeof({descriptor.DeclaredType}), typeof({descriptor.DeclaredType}), ServiceLifetime.{descriptor.Lifetime}));");
                    foreach (var serviceType in descriptor.ServiceTypes)
                    {
                        if (serviceType.Equals(descriptor.DeclaredType) == false)
                        {
                            builder.AppendLine($"        services.TryAddEnumerable(ServiceDescriptor.Describe(typeof({serviceType}), serviceProvider => serviceProvider.GetRequiredService<{descriptor.DeclaredType}>(), ServiceLifetime.{descriptor.Lifetime}));");
                        }
                    }
                }
            }

            if (item.OptionDescriptor != null)
            {
                builder.AppendLine();
                builder.AppendLine($"        // Options");
                builder.AppendLine($"        {item.OptionDescriptor.ToString("services", "configuration")}");
            }
        }

        // dll
        var assemblyName = compilation.AssemblyName;
        var sanitizedAssemblyName = SourceGenerator.GetSanitizedAssemblyName(compilation);
        var output = SourceGenerator.GENERATE_SOURCE.Replace("{0}", assemblyName)
            .Replace("{1}", _className)
            .Replace("{2}", sanitizedAssemblyName)
            .Replace("{3}", builder.ToString());
        context.AddSource(_fileName, SourceText.From(output, Encoding.UTF8));
    }

    private static DependencyInjectionDescriptor? GetDescriptors(GeneratorSyntaxContext context, CancellationToken token)
    {
        var typeDeclarationSyntax = (TypeDeclarationSyntax)context.Node;
        var symbol = context.SemanticModel.GetDeclaredSymbol(typeDeclarationSyntax, token);

        // 服务生命周期标识
        var typeByMetadataNames = SourceGenerator.GetTypeByMetadataNames(context.SemanticModel.Compilation);

        if (symbol != null && symbol is ITypeSymbol typeSymbol)
        {
            DependencyInjectionDescriptor result = new DependencyInjectionDescriptor()
            {
                DeclaredType = typeSymbol
            };

            // 先以接口标识为准，优先级最高
            var lifeCycles = typeSymbol.AllInterfaces.Where(c => typeByMetadataNames.ContainsKey(c.ToDisplayString()));

            // 检测到多个服务生命周期标识
            if (lifeCycles.Count() > 1)
            {
                var messageFormat = $"【{typeSymbol.Name}.cs】继承多个服务生命周期标识 “{string.Join("，", lifeCycles.Select(c => c.ToDisplayString()))}”";
                var diagnostic = SourceGenerator.CreateDiagnostic("AUTODI001", messageFormat, typeSymbol.Locations.FirstOrDefault());
                result.Diagnostic = diagnostic;
                return result;
            }

            // 接口标识注入
            if (lifeCycles.Count() == 1)
            {
                var lifeCycle = lifeCycles.FirstOrDefault();
                var lifetime = lifeCycle.ToDisplayString() switch
                {
                    string value when value.Equals(SourceGenerator.SingletonTypeName) => ServiceLifetime.Singleton,
                    string value when value.Equals(SourceGenerator.TransientTypeName) => ServiceLifetime.Transient,
                    string value when value.Equals(SourceGenerator.ScopedTypeName) => ServiceLifetime.Scoped,
                    _ => throw new InvalidOperationException("Unsupported lifecycle type.")
                };

                var serviceDescriptor = new ServiceDescriptor(lifetime, new TypeSymbol(typeSymbol));
                foreach (var item in typeSymbol.AllInterfaces
                    .Where(c => !typeByMetadataNames.ContainsKey(c.ToDisplayString()))
                    .Where(x => !SourceGenerator.IgnoredInterfaces.Contains(x.ToDisplayString())))
                {
                    if (item is ITypeSymbol serviceType)
                    {
                        serviceDescriptor.ServiceTypes.Add(new TypeSymbol(serviceType));
                    }
                }

                result.ServiceDescriptor = serviceDescriptor;
            }
            // 特性标识注入
            else
            {
                var serviceAttributeTypeName = context.SemanticModel.Compilation.GetTypeByMetadataName(SourceGenerator.ServiceAttributeTypeName);
                var serviceAttributeData = typeSymbol.GetAttributes()
                    .Where(attr => (bool)attr.AttributeClass?.Equals(serviceAttributeTypeName, SymbolEqualityComparer.Default));
                foreach (var attrute in serviceAttributeData)
                {
                    var args = attrute.ConstructorArguments;
                    if (args.Length > 0
                        && args[0].Kind == TypedConstantKind.Enum
                        && args[0].Value is int serviceLifetimeValue
                        && Enum.IsDefined(typeof(ServiceLifetime), serviceLifetimeValue))
                    {
                        var serviceLifetime = (ServiceLifetime)serviceLifetimeValue;
                        var serviceDescriptor = new ServiceDescriptor(serviceLifetime, new TypeSymbol(typeSymbol));

                        for (var i = 1; i < args.Length; i++)
                        {
                            if (args[i].Value is ITypeSymbol serviceType)
                            {
                                serviceDescriptor.ServiceTypes.Add(new TypeSymbol(serviceType));
                            }
                        }

                        result.ServiceDescriptor = serviceDescriptor;
                    }
                }
            }

            // Option
            var optionsAttributeClass = context.SemanticModel.Compilation.GetTypeByMetadataName(SourceGenerator.OptionAttributeTypeName);
            var optionAttributeData = typeSymbol.GetAttributes()
                .Where(attr => attr.AttributeClass?.Equals(optionsAttributeClass, SymbolEqualityComparer.Default) == true);
            foreach (var attrribute in optionAttributeData)
            {
                var attributeClass = attrribute.AttributeClass;
                if (attributeClass != null
                    && attributeClass.Equals(optionsAttributeClass, SymbolEqualityComparer.Default))
                {
                    var args = attrribute.ConstructorArguments;
                    var declaredType = new TypeSymbol(typeSymbol);

                    var optionDescriptor = args.Length > 0 && args[0].Value is string section
                           ? new OptionDescriptor(declaredType, section)
                           : new OptionDescriptor(declaredType, typeSymbol.Name);

                    result.OptionDescriptor = optionDescriptor;
                }
            }

            return result;
        }

        return default;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();
        var autoRegistered = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (syntaxNode, _) => (syntaxNode is ClassDeclarationSyntax) || (syntaxNode is RecordDeclarationSyntax)
            , transform: static (context, token) => GetDescriptors(context, token))
          .Where(autoRegisteredClass => autoRegisteredClass != null);

        // 合并诊断上下文
        context.RegisterSourceOutput(autoRegistered.Collect(), (productionContext, typeSymbol) =>
        {
            foreach (var symbol in typeSymbol)
            {
                if (symbol!.Diagnostic != null)
                {
                    productionContext.ReportDiagnostic(symbol.Diagnostic);
                }
            }
        });

        var compilationModel = context.CompilationProvider.Combine(autoRegistered.Collect());
        context.RegisterSourceOutput(compilationModel, static (sourceContext, source) =>
        {
#pragma warning disable CS8620 // 由于引用类型的可为 null 性差异，实参不能用于形参。
            Execute(source.Left, source.Right, sourceContext);
#pragma warning restore CS8620 // 由于引用类型的可为 null 性差异，实参不能用于形参。
        });
    }
}