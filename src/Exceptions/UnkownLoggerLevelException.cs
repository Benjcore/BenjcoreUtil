using System;

namespace util.Exceptions {
    class UnkownLoggerLevelException : Exception {

        public UnkownLoggerLevelException() {}

        public UnkownLoggerLevelException(string msg) : base(msg) {}

    }
}
