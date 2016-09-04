namespace Dxw.Throttling.Core.Rules
{
    using System.Threading.Tasks;

    public interface IRule : INamed { }

    public interface IRule<in TArg, TRes> : IRule
    {
        IApplyResult<TRes> Apply(TArg context = default(TArg));

        Task<IApplyResult<TRes>> ApplyAsync(TArg context = default(TArg));
    }
}
