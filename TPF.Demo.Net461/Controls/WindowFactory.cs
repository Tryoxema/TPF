using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;

namespace TPF.Demo.Net461.Controls
{
    public static class WindowFactory
    {
        static WindowFactory()
        {
            _allWindows = new Dictionary<Type, WindowMetadataAttribute>();

            _activeWindows = new List<Window>();

            LoadWindows();
        }

        static readonly Dictionary<Type, WindowMetadataAttribute> _allWindows;

        static readonly List<Window> _activeWindows;

        /// <summary>
        /// Lädt alle Windows mit Metadaten in den Cache der WindowFactory
        /// </summary>
        static void LoadWindows()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
                if (assemblies[i].IsDynamic) continue;

                var types = assemblies[i].GetExportedTypes();

                for (var j = 0; j < types.Length; j++)
                {
                    Type type = types[j];

                    var attribute = type.GetCustomAttribute<WindowMetadataAttribute>();

                    if (attribute != null && !_allWindows.TryGetValue(type, out var windowAttribute))
                    {
                        _allWindows.Add(type, attribute);
                    }
                }
            }
        }

        /// <summary>
        /// Schließt alle von der WindowFactory erstellten Fenster
        /// </summary>
        public static void CloseAllWindows()
        {
            // Liste rückwärts abarbeiten, um Probleme mit dem Count-Wert zu verhindern
            for (int i = _activeWindows.Count - 1; i >= 0; i--)
            {
                var window = _activeWindows[i];

                window.Close();
            }
        }

        /// <summary>
        /// Erstellt ein Window-Objekt anhand des Namens
        /// </summary>
        /// <param name="name">Der Name des Fensters</param>
        /// <returns></returns>
        public static Window CreateWindow(string name)
        {
            return CreateWindow(Application.Current.MainWindow, name);
        }

        /// <summary>
        /// Erstellt ein Window-Objekt anhand des Namens und setzt den Owner
        /// </summary>
        /// <param name="owner">Der Owner des Windows</param>
        /// <param name="name">Der Name des Fensters</param>
        /// <returns></returns>
        public static Window CreateWindow(Window owner, string name)
        {
            return CreateWindow(owner, name, null);
        }

        /// <summary>
        /// Erstellt ein Window-Objekt anhand des Namens, setzt den Owner und fügt eine Funktion zum Closed-Event hinzu
        /// </summary>
        /// <param name="owner">Der Owner des Windows</param>
        /// <param name="name">Der Name des Fensters</param>
        /// <param name="onClosed">Die Funktion die an das Closed-Event angehängt werden soll</param>
        /// <returns></returns>
        public static Window CreateWindow(Window owner, string name, Action<Window, bool?> onClosed)
        {
            var windowInfo = _allWindows.FirstOrDefault(i => i.Value.Name == name);

            var windowType = windowInfo.Key;

            // Wurde ein Type gefunden?
            if (windowType != null)
            {
                // Eine Instanz des Windows erstellen
                var window = (Window)Activator.CreateInstance(windowType);

                // Den Owner setzen
                window.Owner = owner;

                switch (windowInfo.Value.WindowType)
                {
                    case WindowType.Normal:
                        window.Show();
                        break;
                    case WindowType.Modal:
                        window.ShowDialog();
                        break;
                    case WindowType.Hidden:
                        break;
                }

                // Funktion an Closed-Event anhängen
                window.Closed += (s, e) =>
                {
                    // Geschlossenes Window aus interner Liste entfernen
                    _activeWindows.Remove(window);
                    // Falls vorhanden die Funktion ausführen
                    onClosed?.Invoke(window, window.DialogResult);
                };

                // Neues Window in interne Liste anhängen
                _activeWindows.Add(window);

                return window;
            }
            else return null;
        }
    }
}