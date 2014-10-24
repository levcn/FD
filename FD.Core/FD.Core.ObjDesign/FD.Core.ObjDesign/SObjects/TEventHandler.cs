namespace ProjectCreater.SObjects
{
    public delegate void TEventHandler<in TSender, in TEventArgs1>(TSender sender, TEventArgs1 args);

    public class DeleteBeforeEventArgs
    {
        public DeleteBeforeEventArgs(object item, bool allowDelete = true)
        {
            Item = item;
            AllowDelete = allowDelete;
        }

        public object Item { get; set; }
        public bool? AllowDelete { get; set; }
    }
}