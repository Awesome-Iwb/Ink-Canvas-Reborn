using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {
    /// <summary>
    /// 配置文件工具集，提供加载、保存和管理配置文件的实用方法。
    /// 修改自 ClassIsland。
    /// 
    /// A utility class for configuration file management, including loading, saving,
    /// and handling configuration files. Modified from ClassIsland.
    /// </summary>
    public class ConfigureFileHelper {
        /// <summary>
        /// 配置文件是否默认启用备份功能。
        /// 
        /// Indicates whether configuration file backups are enabled by default.
        /// </summary>
        public static bool IsBackupEnabled { get; set; } = true;

        /// <summary>
        /// 加载配置文件，并自动创建备份。当加载失败时，将尝试加载备份的配置文件。
        /// 
        /// Loads the configuration file and automatically creates a backup. 
        /// If loading fails, attempts to load the backup configuration file.
        /// </summary>
        /// <typeparam name="T">配置文件的类型。</typeparam>
        /// <param name="path">配置文件路径。</param>
        /// <param name="backupEnabled">是否启用备份功能。如果未指定，使用默认值。</param>
        /// <param name="isLoadingBackup">是否正在加载备份文件。如果是，在加载失败时不会再次尝试加载备份。</param>
        /// <returns>加载的配置文件对象。</returns>
        public static T LoadConfig<T>(string path, bool? backupEnabled = null, bool isLoadingBackup = false) {
            backupEnabled ??= IsBackupEnabled;

            if (!File.Exists(path)) {
                var cfg = Activator.CreateInstance<T>();
                if (!isLoadingBackup)
                    SaveConfig(path, cfg);
                return cfg;
            }

            try {
                var json = File.ReadAllText(path);
                var r = JsonSerializer.Deserialize<T>(json);
                if (r == null)
                    return Activator.CreateInstance<T>();
                if (backupEnabled.Value)
                    File.Copy(path, path + ".bak", true);
                return r;
            }
            catch (Exception) {
                if (!backupEnabled.Value)
                    throw;
                var r = LoadConfig<T>(path + ".bak", false, true);
                File.Copy(path + ".bak", path, true);
                return r;
            }
        }

        /// <summary>
        /// LoadConfig 的精简版，只读取配置文件，不进行备份。
        /// 
        /// A simplified version of LoadConfig that reads the configuration file without creating backups.
        /// </summary>
        /// <typeparam name="T">配置文件的类型。</typeparam>
        /// <param name="path">配置文件路径。</param>
        /// <returns>加载的配置文件对象。</returns>
        public static T ReadConfig<T>(string path) {
            try {
                var json = File.ReadAllText(path);
                var r = JsonSerializer.Deserialize<T>(json);
                return r ?? Activator.CreateInstance<T>();
            }
            catch (Exception) {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 保存配置文件，并自动创建备份。
        /// 
        /// Saves the configuration file and automatically creates a backup.
        /// </summary>
        /// <typeparam name="T">配置文件的类型。</typeparam>
        /// <param name="path">配置文件路径。</param>
        /// <param name="o">要保存到配置文件的对象。</param>
        public static void SaveConfig<T>(string path, T o) {
            WriteAllTextSafe(path, JsonSerializer.Serialize(o));
        }

        /// <summary>
        /// 深度复制一个对象。
        /// 
        /// Creates a deep copy of an object.
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="o">要复制的对象。</param>
        /// <returns>复制后的对象副本。</returns>
        public static T CopyObject<T>(T o) {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(o)) ?? Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 合并字典，将新字典中的元素合并到原字典中，并保持与基准字典的结构一致。
        /// 
        /// Merges dictionaries by comparing the new dictionary with the base dictionary
        /// and ensuring the resulting dictionary matches the base structure.
        /// </summary>
        /// <typeparam name="TKey">字典键的类型。</typeparam>
        /// <typeparam name="TValue">字典值的类型。</typeparam>
        /// <param name="raw">要操作的字典。</param>
        /// <param name="diffBase">基准字典，用于比较。</param>
        /// <param name="diffNew">新字典，提供要更新的数据。</param>
        /// <returns>合并后的字典。</returns>
        public static IDictionary<TKey, TValue> MergeDictionary<TKey, TValue>(IDictionary<TKey, TValue> raw,
            IDictionary<TKey, TValue> diffBase, IDictionary<TKey, TValue> diffNew) {
            var rm = (from i in diffNew.Keys where !diffBase.Keys.Contains(i) select i).ToList();
            rm.ForEach(i => raw.Remove(i));

            foreach (var i in diffNew) {
                if (raw.ContainsKey(i.Key))
                    raw[i.Key] = i.Value;
                else
                    raw.Add(i);
            }

            return raw;
        }

        /// <summary>
        /// 安全地将文本写入文件，确保写入完成。
        /// 
        /// Safely writes text to a file, ensuring the content is fully written.
        /// </summary>
        /// <param name="path">文件路径。</param>
        /// <param name="content">要写入的内容。</param>
        private static void WriteAllTextSafe(string path, string content) {
            using var stream = new FileStream(path, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(content);
            stream.Flush(true);
        }
    }
}
