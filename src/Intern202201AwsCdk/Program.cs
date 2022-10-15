using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
namespace Intern202201AwsCdk
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new Intern202201AwsCdkStack(app, "Intern202201AwsCdkStack");
            app.Synth();
        }
    }
}
