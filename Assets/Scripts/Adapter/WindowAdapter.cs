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
    public class WindowAdapter : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::Window);
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

        public class Adapter : global::Window, CrossBindingAdaptorType
        {
            CrossBindingFunctionInfo<global::UIMsgID, System.Object, System.Object, System.Object, System.Boolean> mOnMessage_0 = new CrossBindingFunctionInfo<global::UIMsgID, System.Object, System.Object, System.Object, System.Boolean>("OnMessage");
            CrossBindingMethodInfo<System.Object, System.Object, System.Object> mAwake_1 = new CrossBindingMethodInfo<System.Object, System.Object, System.Object>("Awake");
            CrossBindingMethodInfo<System.Object, System.Object, System.Object> mOnShow_2 = new CrossBindingMethodInfo<System.Object, System.Object, System.Object>("OnShow");
            CrossBindingMethodInfo mOnDisable_3 = new CrossBindingMethodInfo("OnDisable");
            CrossBindingMethodInfo mOnUpdate_4 = new CrossBindingMethodInfo("OnUpdate");
            CrossBindingMethodInfo mOnFixUpdate_5 = new CrossBindingMethodInfo("OnFixUpdate");
            CrossBindingMethodInfo mOnLateUpdate_6 = new CrossBindingMethodInfo("OnLateUpdate");
            CrossBindingMethodInfo mOnClose_7 = new CrossBindingMethodInfo("OnClose");
            CrossBindingMethodInfo mAddAllEventListener_8 = new CrossBindingMethodInfo("AddAllEventListener");
            CrossBindingMethodInfo mRemoveAllEventListener_9 = new CrossBindingMethodInfo("RemoveAllEventListener");
            CrossBindingMethodInfo mBtnPlaySound_10 = new CrossBindingMethodInfo("BtnPlaySound");

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

            public override System.Boolean OnMessage(global::UIMsgID msgID, System.Object param1, System.Object param2, System.Object param3)
            {
                if (mOnMessage_0.CheckShouldInvokeBase(this.instance))
                    return base.OnMessage(msgID, param1, param2, param3);
                else
                    return mOnMessage_0.Invoke(this.instance, msgID, param1, param2, param3);
            }

            public override void Awake(System.Object param1, System.Object param2, System.Object param3)
            {
                if (mAwake_1.CheckShouldInvokeBase(this.instance))
                    base.Awake(param1, param2, param3);
                else
                    mAwake_1.Invoke(this.instance, param1, param2, param3);
            }

            public override void OnShow(System.Object param1, System.Object param2, System.Object param3)
            {
                if (mOnShow_2.CheckShouldInvokeBase(this.instance))
                    base.OnShow(param1, param2, param3);
                else
                    mOnShow_2.Invoke(this.instance, param1, param2, param3);
            }

            public override void OnDisable()
            {
                if (mOnDisable_3.CheckShouldInvokeBase(this.instance))
                    base.OnDisable();
                else
                    mOnDisable_3.Invoke(this.instance);
            }

            public override void OnUpdate()
            {
                if (mOnUpdate_4.CheckShouldInvokeBase(this.instance))
                    base.OnUpdate();
                else
                    mOnUpdate_4.Invoke(this.instance);
            }

            public override void OnFixUpdate()
            {
                if (mOnFixUpdate_5.CheckShouldInvokeBase(this.instance))
                    base.OnFixUpdate();
                else
                    mOnFixUpdate_5.Invoke(this.instance);
            }

            public override void OnLateUpdate()
            {
                if (mOnLateUpdate_6.CheckShouldInvokeBase(this.instance))
                    base.OnLateUpdate();
                else
                    mOnLateUpdate_6.Invoke(this.instance);
            }

            public override void OnClose()
            {
                if (mOnClose_7.CheckShouldInvokeBase(this.instance))
                    base.OnClose();
                else
                    mOnClose_7.Invoke(this.instance);
            }

            public override void AddAllEventListener()
            {
                if (mAddAllEventListener_8.CheckShouldInvokeBase(this.instance))
                    base.AddAllEventListener();
                else
                    mAddAllEventListener_8.Invoke(this.instance);
            }

            public override void RemoveAllEventListener()
            {
                if (mRemoveAllEventListener_9.CheckShouldInvokeBase(this.instance))
                    base.RemoveAllEventListener();
                else
                    mRemoveAllEventListener_9.Invoke(this.instance);
            }

            protected override void BtnPlaySound()
            {
                if (mBtnPlaySound_10.CheckShouldInvokeBase(this.instance))
                    base.BtnPlaySound();
                else
                    mBtnPlaySound_10.Invoke(this.instance);
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

