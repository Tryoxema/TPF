using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace TPF.Controls
{
    public class DesktopAlertManager
    {
        public DesktopAlertManager(DesktopAlertPosition position = DesktopAlertPosition.BottomRight, double spacing = 8, double margin = 16)
        {
            _position = position;
            _spacing = spacing;
            _margin = margin;
        }

        private readonly List<DesktopAlertHost> _activeHosts = new List<DesktopAlertHost>();
        private readonly DesktopAlertPosition _position;
        private readonly double _spacing;
        private readonly double _margin;

        public DesktopAlert ShowAlert(DesktopAlertConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            // Stelle sicher, dass wir auf dem UI Thread sind
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                return Application.Current.Dispatcher.Invoke(() => ShowAlert(config));
            }

            var alert = CreateAlertFromConfig(config);
            ShowAlert(alert, config.Duration);
            return alert;
        }

        public void ShowAlert(DesktopAlert alert, TimeSpan? duration = null)
        {
            if (alert == null) throw new ArgumentNullException(nameof(alert));

            // Stelle sicher, dass wir auf dem UI Thread sind
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(() => ShowAlert(alert, duration));
                return;
            }

            var host = new DesktopAlertHost(alert, this, duration);
            _activeHosts.Add(host);

            host.Closed += OnHostClosed;
            host.Show();

            RepositionAll();
        }

        private static DesktopAlert CreateAlertFromConfig(DesktopAlertConfig config)
        {
            var alert = new DesktopAlert
            {
                Header = config.Header,
                Content = config.Content,
                Command = config.Command,
                CommandParameter = config.CommandParameter,
                IsClosable = config.IsClosable
            };

            if (config.HeaderTemplate != null) alert.HeaderTemplate = config.HeaderTemplate;
            if (config.ContentTemplate != null) alert.ContentTemplate = config.ContentTemplate;
            if (config.Style != null) alert.Style = config.Style;
            if (config.Background != null) alert.Background = config.Background;
            if (config.Foreground != null) alert.Foreground = config.Foreground;
            
            if (config.OnClick != null) alert.Clicked += (_, __) => config.OnClick();

            // OnClosed: Wir brauchen einen Hook auf das tatsächliche Schließen.
            // Da der Manager bereits OnHostClosed hat, koppeln wir uns an CloseRequested bzw. - sauberer - an das Window.Closed des AlertHosts.
            if (config.OnClosed != null)
            {
                void handler(object _, EventArgs __)
                {
                    config.OnClosed();
                    alert.Closed -= handler;
                }

                alert.Closed += handler;
            }

            return alert;
        }

        private void OnHostClosed(object sender, EventArgs e)
        {
            if (sender is DesktopAlertHost host)
            {
                host.Closed -= OnHostClosed;
                _activeHosts.Remove(host);
                RepositionAll();
            }
        }

        private void RepositionAll()
        {
            // Arbeitsbereich des primären Bildschirms
            var workArea = SystemParameters.WorkArea;

            double offset = _margin;

            // Bei Bottom-* fängt der neueste oben (näher am Rand) an,
            // bei Top-* eben unten - aber weil wir am gleichen Rand stapeln, gehen wir die Liste in normaler Reihenfolge durch und addieren Höhen.
            var isBottom = _position == DesktopAlertPosition.BottomLeft
                        || _position == DesktopAlertPosition.BottomRight
                        || _position == DesktopAlertPosition.BottomCenter;


            var ordered = isBottom
                ? _activeHosts.AsEnumerable().Reverse()
                : _activeHosts.AsEnumerable();

            foreach (var host in ordered)
            {
                // Die ActualHeight steht erst nach Layout zur Verfügung – wenn nicht, fällt's auf MinHeight zurück.
                host.UpdateLayout();
                var width = host.ActualWidth > 0 ? host.ActualWidth : host.Width;
                var height = host.ActualHeight > 0 ? host.ActualHeight : 100;

                double left, top;

                switch (_position)
                {
                    case DesktopAlertPosition.TopLeft:
                    {
                        left = workArea.Left + _margin;
                        top = workArea.Top + offset;
                        break;
                    }
                    case DesktopAlertPosition.TopRight:
                    {
                        left = workArea.Right - width - _margin;
                        top = workArea.Top + offset;
                        break;
                    }
                    case DesktopAlertPosition.TopCenter:
                    {
                        left = workArea.Left + (workArea.Width - width) / 2;
                        top = workArea.Top + offset;
                        break;
                    }
                    case DesktopAlertPosition.BottomLeft:
                    {
                        left = workArea.Left + _margin;
                        top = workArea.Bottom - height - offset;
                        break;
                    }
                    case DesktopAlertPosition.BottomCenter:
                    {
                        left = workArea.Left + (workArea.Width - width) / 2;
                        top = workArea.Bottom - height - offset;
                        break;
                    }
                    case DesktopAlertPosition.BottomRight:
                    default:
                    {
                        left = workArea.Right - width - _margin;
                        top = workArea.Bottom - height - offset;
                        break;
                    }
                }

                host.MoveTo(left, top);
                offset += height + _spacing;
            }
        }

        public void CloseAll()
        {
            var hosts = _activeHosts.ToList();

            for (var i = 0; i < hosts.Count; i++)
            {
                var host = hosts[i];

                host.BeginClose();
            }
        }
    }
}