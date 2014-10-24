using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SLControls.ActiveMethod;
using SLControls.DataClientTools;
using SLControls.Editors;
using SLControls.Extends;
using StaffTrain.FwClass.DataClientTools;
using StaffTrain.FwClass.Reflectors;
using StaffTrain.FwClass.Serializer;
using STComponse.CFG;


namespace SLControls.Controls
{
  [TemplateVisualState(GroupName = "CommonStates", Name = "MouseNormal")]
  [TemplateVisualState(GroupName = "CommonStates", Name = "MouseOver")]
    public class TestMControl2 : BaseMultiControl
    {
//      ScaleTransform scaleTransform = new ScaleTransform();
      public TestMControl2()
        {
            this.DefaultStyleKey = typeof(TestMControl2);
//                scaleTransform.ScaleX = 0.8;
//                scaleTransform.ScaleY = 0.8;
//                RenderTransform = scaleTransform;
        }

      public static readonly DependencyProperty Text1Property =
              DependencyProperty.Register("Text1", typeof(string), typeof(TestMControl2), new PropertyMetadata(default(string), (s, e) =>
              {
                  ((TestMControl2)s).OnText1Changed(e.NewValue as string);
              }));

      private void OnText1Changed(string list)
      {}
      [Editable(GroupName = "基本属性", DisplayName = "文本设置2", Description = "文本设置。")]
      public string Text1
      {
          get
          {
              return (string) GetValue(Text1Property);
          }
          set
          {
              SetValue(Text1Property, value);
          }
      }
      public override void LoadConfig(string configStr)
      {
          var config = JsonHelper.JsonDeserialize<TestMControl2Config>(configStr);
          if (config != null)
          {
          }
      }
    }

  public class TestMControl2Config
    {
        public bool ShowValidCode { get; set; }

        public string ELogin { get; set; }
    }
}
