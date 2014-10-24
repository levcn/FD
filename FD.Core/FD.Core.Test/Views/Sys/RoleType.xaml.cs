using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows;
using Telerik.Windows.Controls;


namespace FD.Core.Test.Views.Sys
{
    public partial class RoleType 
    {
//        RoleTypeModel model = new RoleTypeModel();
        public RoleType()
        {
            InitializeComponent();
//            DataContext = model;
        }
        
    }

    public static class EventService
    {
        
    }
    internal class WeakEventListener<TInstance, TSource, TEventArgs> where TInstance : class
    {
        /// <summary>
        /// WeakReference to the instance listening for the event.
        /// </summary>
        private WeakReference weakInstance;

        /// <summary>
        /// Initializes a new instance of the WeakEventListener class.
        /// </summary>
        /// <param name="instance">Instance subscribing to the event.</param>
        public WeakEventListener(TInstance instance)
        {
            if (null == instance)
            {
                throw new ArgumentNullException("instance");
            }
            this.weakInstance = new WeakReference(instance);
        }

        /// <summary>
        /// Gets or sets the method to call when the event fires.
        /// </summary>
        public Action<TInstance, TSource, TEventArgs> OnEventAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the method to call when detaching from the event.
        /// </summary>
        public Action<WeakEventListener<TInstance, TSource, TEventArgs>> OnDetachAction
        {
            get;
            set;
        }

        /// <summary>
        /// Handler for the subscribed event calls OnEventAction to handle it.
        /// </summary>
        /// <param name="source">Event source.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void OnEvent(TSource source, TEventArgs eventArgs)
        {
            TInstance target = (TInstance)this.weakInstance.Target;
            if (null != target)
            {
                // Call registered action
                if (null != this.OnEventAction)
                {
                    this.OnEventAction(target, source, eventArgs);
                }
            }
            else
            {
                // Detach from event
                this.Detach();
            }
        }

        /// <summary>
        /// Detaches from the subscribed event.
        /// </summary>
        public void Detach()
        {
            if (null != this.OnDetachAction)
            {
                this.OnDetachAction(this);
                this.OnDetachAction = null;
            }
        }
    }
}
