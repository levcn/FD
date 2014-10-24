namespace Fw.ActionMethod
{
    public delegate object ActionExecutor(IController controller, object[] parameters);
    public delegate void VoidActionExecutor(IController controller, object[] parameters);
}