//using System;
//using System.Collections.Generic;
//using CoreStarter.Api.Models;
//using Swashbuckle.AspNetCore.Filters;

//namespace CoreStarter.Api.SwaggerExamples
//{
//    public class FooListResponseExample : IExamplesProvider
//    {
//        public object GetExamples()
//        {
//            return new List<Foo>
//            {
//                new Foo { Id = new Random().Next(), Value = Guid.NewGuid().ToString().Remove(6)},
//                new Foo { Id = new Random().Next(), Value = Guid.NewGuid().ToString().Remove(6)}
//            };
//        }
//    }
//}