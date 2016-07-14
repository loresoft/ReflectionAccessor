using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ReflectionAccessor.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to begin");
            Console.ReadKey();

            var summary = BenchmarkRunner.Run<ReflectionTechnique>();
            Console.WriteLine(summary);
        }
    }

    public class ReflectionTechnique
    {
        private readonly Lazy<PropertyInfo> _firstNameProperty;
        private readonly Lazy<Action<object, object>> _firstNameSetDynamic;
        private readonly Lazy<Func<object, object>> _firstNameGetDynamic;
        private readonly Lazy<Action<object, object>> _firstNameSetExpression;
        private readonly Lazy<Func<object, object>> _firstNameGetExpression;

        private readonly Lazy<IPropertyDelegate> _firstNameDelegate;


        public ReflectionTechnique()
        {
            _firstNameProperty = new Lazy<PropertyInfo>(() => typeof(Contact).GetProperty("FirstName"));
            _firstNameSetDynamic = new Lazy<Action<object, object>>(() => DynamicMethodFactory.CreateSet(_firstNameProperty.Value));
            _firstNameGetDynamic = new Lazy<Func<object, object>>(() => DynamicMethodFactory.CreateGet(_firstNameProperty.Value));
            _firstNameSetExpression = new Lazy<Action<object, object>>(() => ExpressionFactory.CreateSet(_firstNameProperty.Value));
            _firstNameGetExpression = new Lazy<Func<object, object>>(() => ExpressionFactory.CreateGet(_firstNameProperty.Value));

            _firstNameDelegate = new Lazy<IPropertyDelegate>(() => new PropertyDelegate<Contact, string>(_firstNameProperty.Value));

        }


        [Benchmark]
        public void SetByNative()
        {

            var i = new Contact();
            var n = "Name";

            i.FirstName = n;
        }

        [Benchmark]
        public void GetByNative()
        {

            var i = new Contact { FirstName = "Name" };
            var n = i.FirstName;
        }

        [Benchmark]
        public void SetByPropertyInfo()
        {

            var i = new Contact();
            var n = "Name";

            _firstNameProperty.Value.SetValue(i, n);
        }

        [Benchmark]
        public void GetByPropertyInfo()
        {

            var i = new Contact { FirstName = "Name" };
            var n = _firstNameProperty.Value.GetValue(i);
        }

        [Benchmark]
        public void SetByDynamicMethod()
        {

            var i = new Contact();
            var n = "Name";

            _firstNameSetDynamic.Value.Invoke(i, n);
        }

        [Benchmark]
        public void GetByDynamicMethod()
        {

            var i = new Contact { FirstName = "Name" };
            var n = _firstNameGetDynamic.Value.Invoke(i);
        }


        [Benchmark]
        public void SetByExpressionMethod()
        {

            var i = new Contact();
            var n = "Name";

            _firstNameSetExpression.Value.Invoke(i, n);
        }


        [Benchmark]
        public void GetByExpressionMethod()
        {

            var i = new Contact { FirstName = "Name" };
            var n = _firstNameGetExpression.Value.Invoke(i);
        }


        [Benchmark]
        public void SetByDelegateMethod()
        {

            var i = new Contact();
            var n = "Name";

            _firstNameDelegate.Value.SetValue(i, n);
        }


        [Benchmark]
        public void GetByDelegateMethod()
        {

            var i = new Contact { FirstName = "Name" };
            var n = _firstNameDelegate.Value.GetValue(i);
        }


    }
}
