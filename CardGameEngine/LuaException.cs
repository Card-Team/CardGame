using System;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Debugging;

namespace CardGameEngine
{
    public class LuaException : Exception
    {
        public ScriptRuntimeException RuntimeException { get; }
        public List<WatchItem> CallStack { get; }

        internal LuaException(ScriptRuntimeException runtimeException, List<WatchItem> callStack)
        {
            RuntimeException = runtimeException;
            CallStack = callStack;
        }
    }
}