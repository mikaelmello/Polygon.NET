using System;

namespace PolygonNET.Network.Exceptions {
    public class FailedRequestException : Exception {
        public FailedRequestException(string message) : base(message) { }
    }
}