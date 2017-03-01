using JellyFish;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelliumClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DAO dao = new DAO();
        public static TestDetails tds;
        public MainWindow()
        {
            dao.createConnection();
            tds = SerializeDe.DeSerializeObject<TestDetails>();
            ObservableCollection<CRMFeature> all_features = dao.getAllFeatures();
            CRMFeatureList = all_features;
            InitializeComponent();
           
            featuresCombo.ItemsSource = CRMFeatureList;

            populateFields(tds);

        }

        private void populateFields(TestDetails tds)
        {
            if (tds == null) return;

            url_textBox.Text = tds.url;

            foreach (ComboBoxItem cbi in env_type_comboBox.Items)
            {
                if(cbi.Content.ToString().Equals(tds.env_type))
                {
                    cbi.IsSelected = true;
                    break;
                }
            }

            build_bo_textBox1.Text = tds.buildNo;
            mc_name_textBox2.Text = tds.machineName;
            os_textBox3.Text = tds.OS;

            foreach (ComboBoxItem cbi in browser_comboBox.Items)
            {
                if (cbi.Content.ToString().Equals(tds.browser))
                {
                    cbi.IsSelected = true;
                    break;
                }
            }

            alias_textBox4.Text = tds.desc;
        }

        void select_xml_file(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;

            int id = (int)button.Tag;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML files (*.xml)|*.xml";
            
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;

                foreach (TestCase tc in TestCaseList)
                {
                    if (tc.ID == id)
                    {
                        tc.XmlFileName = filename;
                    }
                }
                dataGrid.ItemsSource = TestCaseList;
                try
                {
                    dataGrid.Items.Refresh();
                }
                catch (Exception ) { }
            }            
        }

        
        private ObservableCollection<TestCase> _testCaseList = new ObservableCollection<TestCase>();
        public ObservableCollection<TestCase> TestCaseList
        {
            get { return this._testCaseList; }
            set
            {
                this._testCaseList = value;
            }
        }

        private ObservableCollection<CRMFeature> _CRMFeatureList = new ObservableCollection<CRMFeature>();
        public ObservableCollection<CRMFeature> CRMFeatureList
        {
            get { return this._CRMFeatureList; }
            set
            {
                this._CRMFeatureList = value;
            }
        }
        

        private void featuresCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox features_list = (ComboBox)sender;
            CRMFeature selected_feature = (CRMFeature)features_list.SelectedItem;

            ObservableCollection<TestCase> _testCaseList = dao.getAll_TestCases(selected_feature);

            TestCaseList = _testCaseList;
            dataGrid.ItemsSource = TestCaseList;
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            List<TestCase> selected_testcases = new List<TestCase>();
            foreach (TestCase tc in dataGrid.Items)
            {
                //Console.WriteLine(tc.Seq + "-" + tc.ID + "-" + tc.TestCaseName + "-" + tc.Itr + "-" + tc.Markers + "-" + tc.XmlFileName);
                if (tc.XmlFileName != null && tc.XmlFileName.Length > 0)
                {
                    selected_testcases.Add(tc);
                }
            }
            List<TestCase> SortedList_selected_testcases = selected_testcases.OrderBy(o => o.Seq).ToList();

            TestDetails tds = new TestDetails();

            tds.url = url_textBox.Text;
            tds.env_type = (String)((ComboBoxItem)env_type_comboBox.SelectedValue).Content;
            tds.buildNo = build_bo_textBox1.Text;
            tds.machineName = mc_name_textBox2.Text;
            tds.OS = os_textBox3.Text;
            tds.browser = (String)((ComboBoxItem)browser_comboBox.SelectedValue).Content;
            tds.desc = alias_textBox4.Text;

            int isValid = Validate(tds, SortedList_selected_testcases);

            if (isValid == 0)
            {
                MessageBoxResult result = MessageBox.Show("All fields in environment details are mendatory.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (isValid == 1)
            {
                MessageBoxResult result = MessageBox.Show("Please provide XML test case file.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SerializeDe.SerializeObject(tds);

           
            
            ProgressWindow pw = new ProgressWindow(SortedList_selected_testcases);
            pw.Owner = this;
            this.Hide(); 
            pw.ShowDialog();
        }

        private int Validate(TestDetails tds, List<TestCase> sortedList_selected_testcases)
        {
            if (tds.browser == null || tds.browser.Length == 0 ||
                tds.buildNo == null || tds.buildNo.Length == 0 ||
                tds.desc == null || tds.desc.Length == 0 ||
                tds.env_type == null || tds.env_type.Length == 0 ||
                tds.machineName == null || tds.machineName.Length == 0 ||
                tds.OS == null || tds.OS.Length == 0 ||
                tds.url == null || tds.url.Length == 0 )
            {
                return 0;
            }
            
            if(sortedList_selected_testcases.Count == 0)
            {
                return 1;
            }
            return 2;
        }
    }
}
