﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.UnitTests
{
    using System;
    using System.Net.Http;
    using System.Web;

    using Moq;

    public static class RequestCreater
    {
        public static HttpRequestMessage CreateRequest(string url, HttpMethod method, string ip)
        {
            var request = new HttpRequestMessage();

            var baseRequest = new Mock<HttpRequestBase>(MockBehavior.Strict);
            var baseContext = new Mock<HttpContextBase>(MockBehavior.Strict);

            baseRequest.Setup(br => br.UserHostAddress).Returns(ip);
            baseContext.Setup(bc => bc.Request).Returns(baseRequest.Object);

            request.RequestUri = new Uri(url);

            request.Properties.Add("MS_HttpContext", baseContext.Object);

            request.Method = method;
            return request;
        }
    }
}
