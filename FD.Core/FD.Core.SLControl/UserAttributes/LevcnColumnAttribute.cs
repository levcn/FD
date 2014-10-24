using System;
using System.Windows.Data;
//using StaffTrain.FwClass.Controls.EditItems;


namespace StaffTrain.FwClass.UserAttributes
{
    /// <summary>
    /// �ֶ���Ϣ����
    /// </summary>
    public class LevcnColumnAttribute : Attribute
    {
        /// <summary>
        /// ���ʱ���Ըı�,ȷ���Ժ����޸�
        /// </summary>
        public bool InitialEdit { get; set; }
        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// ��ǰ�ֶ�����,Ĭ��ȡ������Ϊ�ֶ��� 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ʾ��,���ҳ�治��Ҫ�༭����Բ�д
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// �Ƿ����,�������ݿ��в�ѯ
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// �ڱ༭ҳ���ϵ���ʾ˳��
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// �Ƿ���Ա༭
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// �������Ƿ��ֹ�����
        /// </summary>
        public bool CustomInput { get; set; }

        /// <summary>
        /// �ڱ༭ҳ���Ƿ�����
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// �������Ƿ����ڹ�������ʵ������
        /// </summary>
        public bool IsFlag { get; set; }

        /// <summary>
        /// ��ǰ�����Ƿ�����ʾ����������
        /// </summary>
        public bool IsDisplayName { get; set; }

      
        /// <summary>
        /// ʹ�ñ༭�ؼ�������
        /// </summary>
        public Type ControlType { get; set; }
        /// <summary>
        /// �ļ��ϴ�֮���ŵĴ�������
        /// �����ļ��ϴ�֮��Ĵ�����Ż���
        /// </summary>
        public string FileType { get; set; }
        /// <summary>
        /// �����ϴ����ļ�����
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// ���ʹ��������,���������������Դ
        /// </summary>
        public string ListSourceName { get; set; }

        /// <summary>
        /// ��������Դ����������
        /// </summary>
        public Type ListSourceType { get; set; }

        /// <summary>
        /// Ҫ�򿪴���ʱ,���ڵĸ�
        /// </summary>
        public int WindowsHeight { get; set; }

        /// <summary>
        /// Ҫ�򿪴���ʱ,���ڵĿ�
        /// </summary>
        public int WindowsWidth { get; set; }

        /// <summary>
        /// Ҫ�򿪴���ʱ,�����Ƿ����ScrollViewer
        /// </summary>
        public bool WindowNoScroll { get; set; }

        /// <summary>
        /// ʹ��ѡ��ؼ�ʱ,ѡ��ؼ�������
        /// </summary>
        public string SelectControlName { get; set; }

        public string FilePath { get; set; }

        /// <summary>
        /// �Ƿ��Ǽ����ַ���
        /// </summary>
        public bool IsPassword { get; set; }

        /// <summary>
        /// ���ֿ��Ƿ���Ի���
        /// </summary>
        public bool AcceptsReturn { get; set; }

        /// <summary>
        /// �ı���ĸ߶�
        /// </summary>
        public int TextBoxHeight { get; set; }

        /// <summary>
        /// �ı���ĸ߶�
        /// </summary>
        public int TextBoxWidth { get; set; }

        /// <summary>
        /// ��ǰID��Ӧ����һ�����Ե�����
        /// </summary>
        public Type OtherPropType { get; set; }

        /// <summary>
        /// ��ǰID��Ӧ����һ�����Ե�����
        /// �б�ѡ��ʱ���ݵĲ������������� 
        /// ����Ӧ�ò��� ��Ա��ɫѡ���Ծ�γ�ѡ��
        /// </summary>
        public string OtherPropName { get; set; }

        /// <summary>
        /// ��ǰ�ֶ��Ƿ��ʾ,�� ��
        /// </summary>
        public bool YesNo { get; set; }

        /// <summary>
        /// ԭʼ�ļ���
        /// </summary>
        public string OriginalFileName { get; set; }

        public string[] ColumnBinding { get; set; }

        public string[] HeaderText { get; set; }

        public Type Converter { get; set; }

        /// <summary>
        /// �б��Ƿ��ǵ���ѡ�������
        /// </summary>
        public bool Addition { get; set; }

        /// <summary>
        /// �Ƿ���ʾ���ڵ�ʱ��
        /// </summary>
        public bool ShowTime { get; set; }

        /// <summary>
        /// ѡ��ҳ����Ҫ�Ĳ�������Ӧ��ʵ���������
        /// EG: CourseID,PassScore
        /// </summary>
        public string ParamPropNames { get; set; }

        #region RichTextBox

        /// <summary>
        /// �����ļ�
        /// </summary>
        public Type SaveFileAction { get; set; }

//        /// <summary>
//        /// �����ļ������URL������
//        /// </summary>
//        public IValueConverter FilePathConverter { get; set; }

        #endregion

    }
}