using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Aspose.Words;
using Aspose.Words.Drawing;


namespace Fw.Documents
{
    public  static partial class AsposeWordsExContent
    {
        /// <summary>
        /// 添加一个线条
        /// </summary>
        /// <param name="owner"></param>
        public static void AppendImageLine(this DocumentBuilder owner)
        {
            var aviliableWidth = (int)(owner.PageSetup.PageWidth - owner.PageSetup.LeftMargin - owner.PageSetup.RightMargin);
            var d = owner.InsertImage(GetImg(aviliableWidth, 1), aviliableWidth, 1);
            d.HorizontalAlignment = HorizontalAlignment.Center;
            d.RelativeVerticalPosition = RelativeVerticalPosition.OutsideMargin;
        }

/// <summary>
        /// 添加一个线条
        /// </summary>
        /// <param name="owner"></param>
        public static int GetAviliableWidth(this DocumentBuilder owner)
        {
            var aviliableWidth = (int)(owner.PageSetup.PageWidth - owner.PageSetup.LeftMargin - owner.PageSetup.RightMargin);
    return aviliableWidth;
        }
        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="owner"></param>
        public static void AppendCatalog(this DocumentBuilder owner)
        {
            owner.InsertTableOfContents(@"\o ""1-3"" \h \z \u");
        }
        /// <summary>
        /// 添加分页符
        /// </summary>
        /// <param name="owner"></param>
        public static void AppendPageBreak(this DocumentBuilder owner)
        {
            owner.InsertBreak(BreakType.PageBreak);
        }
        /// <summary>
        /// 添加换行
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="count"></param>
        public static void AppendEnter(this DocumentBuilder owner,
                                        int count)
        {
            for (int i = 0; i < count; i++)
            {
                owner.InsertParagraph();
            }
        }
        /// <summary>
        /// 添加文本
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="str"></param>
        /// <param name="beforeClearFormat"></param>
        /// <param name="afterClearFormat"></param>
        /// <param name="fontsize"></param>
        /// <param name="fontName"></param>
        /// <param name="bold"></param>
        /// <param name="italic"></param>
        /// <param name="alignment"></param>
        /// <param name="Scaling"></param>
        /// <param name="Spacing"></param>
        /// <param name="beforeEnter"></param>
        /// <param name="afterEnter"></param>
        /// <param name="firstLineIndent"></param>
        /// <param name="color"></param>
        /// <param name="underline"></param>
        public static void AppendTextEx(this DocumentBuilder owner,
            string str,
            bool beforeClearFormat = false,
            bool afterClearFormat = false,
            double? fontsize = null,
            string fontName = null,
            bool bold = false,
            bool italic = false,
            ParagraphAlignment? alignment = default(ParagraphAlignment?),
            int Scaling = 100, //字体广大
            double Spacing = 0,//字体间距
            bool beforeEnter = false,//之前是否换行
            bool afterEnter = false,//完成之后是否换行
            double? firstLineIndent = 0,//完成之后是否换行
            Color? color = null,//完成之后是否换行
            Underline underline = Underline.None
            )
        {
            if (beforeClearFormat) owner.Font.ClearFormatting();
            if (alignment != null) owner.ParagraphFormat.Alignment = alignment.Value;
            if (firstLineIndent != null) owner.ParagraphFormat.FirstLineIndent = firstLineIndent.Value;
            if (color != null) owner.Font.Color = color.Value;
            owner.Font.Scaling = Scaling;
            owner.Font.Spacing = Spacing;
            if (fontsize != null) owner.Font.Size = fontsize.Value;
            if (fontName != null)
            {
                owner.Font.NameOther = owner.Font.NameFarEast = owner.Font.NameBi = owner.Font.NameAscii = owner.Font.Name = fontName;
            }

            owner.Font.Bold = bold;

            owner.Font.Italic = italic;
            owner.Font.Underline = underline;
            if (beforeEnter) owner.Writeln();
            owner.Write(str);
            if (afterEnter) owner.Writeln();
            if (afterClearFormat) owner.Font.ClearFormatting();
        }

        public static Bitmap GetImg(int width, int height)
        {
            Bitmap img = null;
            img = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(img);
            g.Clear(Color.Black);
            g.Dispose();
            return img;
        }
        public static void AppendFile(this DocumentBuilder owner, byte[] fileBytes)
        {
            MemoryStream ms = new MemoryStream(fileBytes);
            owner.AppendFile(new Document(ms));
        }
        public static void AppendFile(this DocumentBuilder owner, string fileName)
        {
            owner.AppendFile(new Document(fileName));
        }
        /// <summary>

        /// Inserts content of the external document after the specified node.

        /// Section breaks and section formatting of the inserted document are ignored.

        /// </summary>

        /// <param name="insertAfterNode">Node in the destination document after which the content 

        /// should be inserted. This node should be a block level node (paragraph or table).</param>

        /// <param name="srcDoc">The document to insert.</param>

//        static void AppendFile(this DocumentBuilder owner, Document srcDoc)
//        {
////            Bookmark bookmark = owner.Document.Range.Bookmarks["insertionPlace"];
//
//            owner.AppendFile(bookmark.BookmarkStart.ParentNode, subDoc);
//
//        }
        static void AppendFile(this DocumentBuilder owner,Node insertAfterNode, Document srcDoc)
        {

            // Make sure that the node is either a paragraph or table.

            if ((!insertAfterNode.NodeType.Equals(NodeType.Paragraph)) &

              (!insertAfterNode.NodeType.Equals(NodeType.Table)))

                throw new ArgumentException("The destination node should be either a paragraph or table.");
            // We will be inserting into the parent of the destination paragraph.

            CompositeNode dstStory = insertAfterNode.ParentNode;



            // This object will be translating styles and lists during the import.

            NodeImporter importer = new NodeImporter(srcDoc, insertAfterNode.Document, ImportFormatMode.KeepSourceFormatting);



            // Loop through all sections in the source document.

            foreach (Section srcSection in srcDoc.Sections)
            {

                // Loop through all block level nodes (paragraphs and tables) in the body of the section.

                foreach (Node srcNode in srcSection.Body)
                {

                    // Let's skip the node if it is a last empty paragraph in a section.

                    if (srcNode.NodeType.Equals(NodeType.Paragraph))
                    {

                        Paragraph para = (Paragraph)srcNode;

                        if (para.IsEndOfSection && !para.HasChildNodes)

                            continue;

                    }



                    // This creates a clone of the node, suitable for insertion into the destination document.

                    Node newNode = importer.ImportNode(srcNode, true);



                    // Insert new node after the reference node.

                    dstStory.InsertAfter(newNode, insertAfterNode);

                    insertAfterNode = newNode;

                }

            }

        }

        /// <summary>
        /// 追加一个文件
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="fileName"></param>
        public static void AppendFile(this DocumentBuilder owner, Document srcDoc)
        {
            owner.MoveToDocumentEnd();
            owner.AppendFile(owner.CurrentParagraph,srcDoc);
            owner.MoveToDocumentEnd();
            return;
            owner.Document.AppendDocument(srcDoc, ImportFormatMode.UseDestinationStyles);
            var erewrw1 = owner.Document.GetChildNodes(NodeType.Any, true).ToArray();

            var section0 = owner.Document.ChildNodes[owner.Document.ChildNodes.Count - 2] as Section;
            var section1 = owner.Document.ChildNodes[owner.Document.ChildNodes.Count - 1] as Section;
            var aa = (section1.ChildNodes.ToArray().First(w => w is Body) as Body).ChildNodes.ToArray();
            var last = aa.Last() as Paragraph;
            if (last != null)
            {
                //                var fer = last.GetChildNodes(NodeType.Any,true).ToArray();
                //                //把最后一段的换页符号去了
                //                last.Range.Replace(new string(new char[] { ((char)36) }), "", false, false);

            }
            var body = (section0.ChildNodes.ToArray().First(w => w is Body) as Body);
            foreach (Node node in aa)
            {
                body.ChildNodes.Add(node);
            }

            owner.Document.ChildNodes.RemoveAt(1);
            owner.MoveToDocumentEnd();
            //            var erewrw1 = db.Document.GetChildNodes(NodeType.Any, true).ToArray();
            //            dddddd(db.Document.ChildNodes);
        }
    }
}