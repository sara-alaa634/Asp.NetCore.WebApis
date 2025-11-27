using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.CommanResult
{
    public class Result
    {
        // IsSuccess
        //IsFailure
        // Errors[Code-Description-Type]=> Class 

        protected readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count ==0;
        public bool IsFailure => !IsSuccess;

        // Property to get Errors
        public IReadOnlyList<Error> Errors => _errors;
        // IReadOnlyList=> allowing getting data only not update or delete or modify

        protected Result()
        {
            
        }

        protected Result(Error error)
        {
            _errors.Add(error);

        }

        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        // MEthods to set values to constructors

        public static Result Ok()=> new Result();

        // If one error occure
        public static Result Fail(Error error)=> new Result(error);

        // if more than error occure
        public static Result Fail(List<Error> errors) => new Result(errors);


        // No Errors =>
        // One Error Occure =>
        // More than one Error Occure =>
    }

    public class  Result<TValue>:Result
    {
        private readonly TValue _value;
        public TValue Value=> IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(TValue value)
        {
            _value = value;
        }

        private Result(Error error):base(error)
        {
            _value=default!;
        }

        private Result(List<Error> errors):base(errors)
        {
            _value=default!;
        }
        

        // Static Factory Methods
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);

        // Implcit Casting
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
    }
}
