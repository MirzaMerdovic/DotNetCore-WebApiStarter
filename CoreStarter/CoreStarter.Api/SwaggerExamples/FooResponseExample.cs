//using System;
//using CoreStarter.Api.Models;
//using Swashbuckle.AspNetCore.Filters;

//namespace CoreStarter.Api.SwaggerExamples
//{
//    public class FooResponseExample : IExamplesProvider
//    {
//        public object GetExamples()
//        {
//            return new Foo
//            {
//                Id = new Random().Next(),
//                Value = Guid.NewGuid().ToString().Remove(6)
//            };
//        }
//    }
//}