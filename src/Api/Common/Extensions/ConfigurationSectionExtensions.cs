using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Api.Common.Extensions {
    public static class ConfigurationSectionExtensions {
        public static void CheckExistPropertiesAndThrow<T>(
            this IConfigurationSection configurationSection
        ) {
            var necessaryProperties = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x =>
                    x.GetGetMethod() != null && x.GetSetMethod() != null)
                .Select(x => x.Name);
            var existProperties =
                configurationSection.GetChildren().Select(x => x.Key);
            var missingProperties = necessaryProperties.Except(existProperties)
                .ToImmutableList();
            if (missingProperties.Any()) {
                throw new Exception(
                    $"В файле конфигурации для секции {configurationSection.Path} " +
                    $"не заданы параметры: {string.Join(", ", missingProperties)}."
                );
            }
        }
    }
}