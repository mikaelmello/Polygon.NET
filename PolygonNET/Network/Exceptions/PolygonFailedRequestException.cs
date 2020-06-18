using System;

namespace PolygonNET.Network.Exceptions {
    public class PolygonFailedRequestException : Exception {
        public PolygonFailedRequestException(string message) : base(message) { }
    }
}
