using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#if DEBUG && !DISABLE_ILRUNTIME_DEBUG
using AutoList = System.Collections.Generic.List<object>;
#else
using AutoList = ILRuntime.Other.UncheckedList<object>;
#endif

namespace HotFix
{   
    public class ObjectParentAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::ObjectParent);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : global::ObjectParent, CrossBindingAdaptorType
        {
            CrossBindingMethodInfo<System.Object, System.Object, System.Object> mAwake_0 = new CrossBindingMethodInfo<System.Object, System.Object, System.Object>("Awake");
            CrossBindingMethodInfo<System.Object, System.Object, System.Object> mOnShow_1 = new CrossBindingMethodInfo<System.Object, System.Object, System.Object>("OnShow");
            CrossBindingMethodInfo mOnDisable_2 = new CrossBindingMethodInfo("OnDisable");
            CrossBindingMethodInfo mOnUpdate_3 = new CrossBindingMethodInfo("OnUpdate");
            CrossBindingMethodInfo mOnFixUpdate_4 = new CrossBindingMethodInfo("OnFixUpdate");
            CrossBindingMethodInfo mOnLateUpdate_5 = new CrossBindingMethodInfo("OnLateUpdate");
            CrossBindingMethodInfo mOnClose_6 = new CrossBindingMethodInfo("OnClose");

            bool isInvokingToString;
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void Awake(System.Object param1, System.Object param2, System.Object param3)
            {
                if (mAwake_0.CheckShouldInvokeBase(this.instance))
                    base.Awake(param1, param2, param3);
                else
                    mAwake_0.Invoke(this.instance, param1, param2, param3);
            }

            public override void OnShow(System.Object param1, System.Object param2, System.Object param3)
            {
                if (mOnShow_1.CheckShouldInvokeBase(this.instance))
                    base.OnShow(param1, param2, param3);
                else
                    mOnShow_1.Invoke(this.instance, param1, param2, param3);
            }

            public override void OnDisable()
            {
                if (mOnDisable_2.CheckShouldInvokeBase(this.instance))
                    base.OnDisable();
                else
                    mOnDisable_2.Invoke(this.instance);
            }

            public override void OnUpdate()
            {
                if (mOnUpdate_3.CheckShouldInvokeBase(this.instance))
                    base.OnUpdate();
                else
                    mOnUpdate_3.Invoke(this.instance);
            }

            public override void OnFixUpdate()
            {
                if (mOnFixUpdate_4.CheckShouldInvokeBase(this.instance))
                    base.OnFixUpdate();
                else
                    mOnFixUpdate_4.Invoke(this.instance);
            }

            public override void OnLateUpdate()
            {
                if (mOnLateUpdate_5.CheckShouldInvokeBase(this.instance))
                    base.OnLateUpdate();
                else
                    mOnLateUpdate_5.Invoke(this.instance);
            }

            public override void OnClose()
            {
                if (mOnClose_6.CheckShouldInvokeBase(this.instance))
                    base.OnClose();
                else
                    mOnClose_6.Invoke(this.instance);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    if (!isInvokingToString)
                    {
                        isInvokingToString = true;
                        string res = instance.ToString();
                        isInvokingToString = false;
                        return res;
                    }
                    else
                        return instance.Type.FullName;
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

