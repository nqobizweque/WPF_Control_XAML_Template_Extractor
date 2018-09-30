using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace ControlTemplateExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ctor
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        

        private List<Type> _controls;
        public List<Type> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        #region Events
        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            Heading.Text = "WPF Control Template Extractor";
            TypeList.ItemsSource = Controls = await Task.Run(() => ControlCollections.GetConfigControls().Where(t => HasResources(t)).ToList());
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var type = Controls[TypeList.SelectedIndex];
                var controls = Controls;
                var control = Application.Current.FindResource(type);
                XmlDocument doc = new XmlDocument();
                StringBuilder stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    XamlWriter.Save(control, xmlWriter);
                    doc.Save(xmlWriter);
                    xmlWriter.Close();
                    DisplayBox.Text = stringBuilder.ToString();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Methods
        private bool HasResources(Type type)
        {
            try
            {
                Application.Current.FindResource(type);
                return true;
            } catch { }
            return false;
        }
        #endregion
    }
}
