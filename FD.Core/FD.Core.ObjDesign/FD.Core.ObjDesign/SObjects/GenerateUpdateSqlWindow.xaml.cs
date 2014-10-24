using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectCreater.Settings;
using STComponse;
using STComponse.CFG;
using STComponse.DB;
using WPFControls;
using MessageBox = System.Windows.MessageBox;


namespace ProjectCreater.SObjects
{
    /// <summary>
    /// AddNewVersionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GenerateUpdateSqlWindow
    {
        public GenerateUpdateSqlWindow()
        {
            InitializeComponent();
            var readOnlyCollection = VersionManage.Current.Versions;
            CB_VersionList1.ItemsSource = readOnlyCollection;
            CB_VersionList2.ItemsSource = readOnlyCollection;
            CB_VersionList1.DisplayMemberPath = "VersionName";
            CB_VersionList1.SelectedValuePath = "VersionNumber";
            CB_VersionList2.DisplayMemberPath = "VersionName";
            CB_VersionList2.SelectedValuePath = "VersionNumber";
            if (readOnlyCollection.Count > 0)
            {
                CB_VersionList1.SelectedIndex = readOnlyCollection.Count - 1;
                CB_VersionList2.SelectedIndex = 0;
            }
        }

        private void OnResult2(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OnResult1(object sender, RoutedEventArgs e)
        {
            try
            {
            var source = (CB_VersionList1.SelectedItem as FwConfig).ToDB();
            var target = (CB_VersionList2.SelectedItem as FwConfig).ToDB();
            
                var text = (string)EDBCompare.Compare(source, target);
                MessageBox.Show(text);
                MainUtils.SetClipboardText(text);
            }
            catch (Exception eee)
            {
                if (eee is PException)
                {
                    MessageBox.Show(eee.Message);
                }
                else
                {
                    var text = eee.GetBaseException().ToString();
                    MessageBox.Show(text);
                }
            }
            DialogResult = true;
        }
    }
}
