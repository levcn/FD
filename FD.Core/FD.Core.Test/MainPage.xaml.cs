using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FD.Core.SLControl.Data;
using FD.Core.SLControl.NavigatorTools;
using FD.Core.Test.Annotations;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;
using STComponse.CFG;
using Telerik.Windows.Controls;


namespace FD.Core.Test
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += sdfsd;
            BaseMultiControl.sss = aaaaaa;
        }

        private async void sdfsd(object sender, RoutedEventArgs e)
        {
            Style style = App.Current.Resources["MainFrameStyle1"] as Style;
            aaa.Style = style;
//            RadC
            var sdf = new Mod
            {
                Name = "123123123",
            };

            sdf.IsSelected = true;
            itemsSource = new List<Mod> {
                new Mod {
                    Name = "Page2", PageUrl = "$DeskTop.xaml", ImageUrl = "Images/study.png", ImageUrl2 = "Images/study2.png",
                },
                new Mod {
                    StyleName = "SubFrame_Style2",
                    IsSelected = true, Name = "Page1", PageUrl = "$SilverlightControl1.xaml", ImageUrl = "Images/Course.png", ImageUrl2 = "Images/Course2.png",
                    Children = new List<INode> {
                        new Mod {
                            Name = "课程资源课程资源", PageUrl = "$SilverlightControl1.xaml",
                        },
new Mod {
                            Name = "课程内容", PageUrl = "$SilverlightControl1.xaml",
                        },
                        new Mod {
                            Name = "课程考试", PageUrl = "$SilverlightControl1.xaml",
                        },

                        new Mod {
                            StyleName = "SubFrame_Style3",

                            Name = "试题", PageUrl = "$SilverlightControl2.xaml",
                            Children = new List<INode> {
                                new Mod {
                                    Name = "报表1报表1报表1", PageUrl = "$SilverlightControl1.xaml",
                                    IsSelected = true,
                                },
                                new Mod {
                                    Name = "报表2", PageUrl = "$SilverlightControl2.xaml",

                                }
                            },
                        },
                        new Mod {
                            Name = "试题列表", PageUrl = "$SilverlightControl2.xaml",
                            Children = new List<INode> {
                                new Mod {
                                    Name = "报表1", PageUrl = "$SilverlightControl1.xaml",
                                },
                                new Mod {
                                    Name = "报表2", PageUrl = "$SilverlightControl2.xaml",

                                }
                            },
                        }
                    },
                },
                new Mod {
//                    StyleName = "SubFrame_Style2",
                    IsSelected = true, Name = "Page1", PageUrl = "$SilverlightControl1.xaml", ImageUrl = "Images/Course.png", ImageUrl2 = "Images/Course2.png",
                    Children = new List<INode> {
                        new Mod {
                            Name = "组织机构", PageUrl = "$SilverlightControl1.xaml",
                        },
                        new Mod {
                            StyleName = "SubFrame_Style2",
                            Name = "角色管理", PageUrl = "$SilverlightControl2.xaml",
                            Children = new List<INode> {
                                new Mod {
                                    Name = "角色类别", PageUrl = "$SilverlightControl1.xaml",
                                },
                                new Mod {
                                    Name = "角色设置", PageUrl = "$SilverlightControl2.xaml",

                                }
                            },
                        },
                        new Mod {
                            Name = "员工管理", PageUrl = "$SilverlightControl1.xaml",
                        },
                        new Mod {
                            Name = "功能管理", PageUrl = "$SilverlightControl1.xaml",
                        },
                    },
                },
                new Mod {
                    Name = "Page2", PageUrl = "$MessageTest.xaml", ImageUrl = "Images/study.png", ImageUrl2 = "Images/study2.png",

                },
                new Mod {
                    Name = "Page3", 
                    PageUrl = "$SilverlightControl2.xaml",
                    ImageUrl = "Images/admin.png", 
                    ImageUrl2 = "Images/admin2.png",
                    StyleName = "SubFrame_Style1",
                     Children = new List<INode> {
                        new Mod {
                            Name = "组织机构", PageUrl = "$Sys.Org.xaml",
                        },
                        new Mod {
                            StyleName = "SubFrame_Style3",
                            Name = "角色管理", PageUrl = "$SilverlightControl2.xaml",
                            Children = new List<INode> {
                                new Mod {
                                    Name = "角色类别", PageUrl = "$Sys.RoleType.xaml",
                                },
                                new Mod {
                                    Name = "角色设置", PageUrl = "$Sys.Role.xaml",
                                }
                            },
                        },
                        new Mod {
                            Name = "员工管理", PageUrl = "$SilverlightControl1.xaml",
                        },
                        new Mod {
                            Name = "功能管理", PageUrl = "$SilverlightControl1.xaml",
                        },
                    },
                }
            };
//            var str = await DataAccess.AsyncReadServerTxtFile(string.Format("Resourse/MenuConfig/config1.txt"));
//            itemsSource = str<List<Mod>>();
//            var stringsdfs = itemsSource.ToXml();
            itemsSource.ForEach(w => w.PropertyChanged += sss);
            aaa.ItemsSource = itemsSource;
            aaa.MenuItems = itemsSource;
        }
        List<Mod> itemsSource;

        private void sss(object sender, PropertyChangedEventArgs e)
        {
            //            PART_MenuItemsControl.ItemsSource = null;
            //            PART_MenuItemsControl.ItemsSource = itemsSource;
        }

        private void Run_OnClick(object sender, RoutedEventArgs e)
        {
            BaseMultiControl.ToRunMode(this);
            return;
            var childs = ChildrenOfTypeExtensions.ChildrenOfType<BaseMultiControl>(this).ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Run;
            });
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            BaseMultiControl.ToEditMode(this);
            return;
            var childs = ChildrenOfTypeExtensions.ChildrenOfType<BaseMultiControl>(this).ToList();
            childs.ForEach(w =>
            {
                w.EditState = EditState.Normal;
            });
        }

        public override string PageCode
        {
            get
            {
                return "MainPage";
            }
        }
    }

    public class Mod : IMenuItem
    {
        public Mod()
        {
            ID = Guid.NewGuid();
        }

        private bool isSelected;
        private string name;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public IList<INode> Children { get; set; }
        public string Text { get; set; }
        public bool IsExpanded { get; set; }
        public BitmapImage Picture { get; set; }
        public string PicUrl { get; set; }

        public void Add(INode node)
        { }

        public void Delete(INode node)
        { }

        public Guid ID { get; set; }
        public string PageUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrl2 { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public string Code { get; set; }
        public int OrderID { get; set; }
        public string Memo { get; set; }
        public string StyleName { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
