using PostSharp.Aspects;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CacheUtil
{
    [Serializable]
    public class CacheableAttribute : MethodInterceptionAspect
    {
        private Type Tipo { get; set; }
        private int ExpirarEmMinutos { get; set; }

        public CacheableAttribute(Type tipo) :this (tipo, 60)
        { }
        public CacheableAttribute(Type tipo, int expirarEmMinutos)
		{
            Tipo = tipo;
            ExpirarEmMinutos = expirarEmMinutos;
		}

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string chave = string.Format("{0}, {1}", args.Method, serializer.Serialize(args.Arguments.ToArray()));

            var info = Caching.Instancia.Retornar(chave);
            
            if (info != null)
            {
                args.ReturnValue = serializer.Deserialize((string)info, Tipo);
                return;
            }

            base.OnInvoke(args);
            Caching.Instancia.Definir(chave, serializer.Serialize(args.ReturnValue), new TimeSpan(0, ExpirarEmMinutos, 0));
        }
    }
}
