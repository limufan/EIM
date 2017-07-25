using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    public interface IKeyProvider
    {
        object GetKey();
    }

    public interface IIdProvider
    {
        int Id { get; }
    }

    public interface ICodeProvider
    {
        string Code { get; }
    }

    public interface IGuidProvider
    {
        string Guid { get; }
    }

    public interface ILongCodeProvider
    {
        string LongCode { get; }
    }

    public interface INameProvider
    {
        string Name { get; }
    }

    public interface IIdCodeProvider : ICodeProvider, IIdProvider
    {

    }

    public interface IGuidCodeProvider : ICodeProvider, IGuidProvider
    {

    }

    public interface IIdGuidProvider : IGuidProvider, IIdProvider
    {

    }

    public interface IIdCodeNameProvider : IIdCodeProvider, INameProvider
    {
        
    }

    public interface IIdCodeGuidProvider : IIdCodeProvider, IGuidProvider
    {
        
    }

    public interface IIdCodeLongCodeNameProvider : IIdCodeLongCodeProvider, INameProvider
    {
        
    }

    public interface IIdCodeLongCodeProvider : IIdCodeProvider, ILongCodeProvider
    {
        
    }

}
