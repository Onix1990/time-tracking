using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common.ActionFilters {
    [AttributeUsage(AttributeTargets.Method)]
    public class UowTypeAttribute : TypeFilterAttribute {
        public UowTypeAttribute(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted
        ) : base(typeof(UowActionFilter)) {
            Arguments = new object[] {isolationLevel};
        }
    }
}