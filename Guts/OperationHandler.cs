using Guts.Excel;

namespace Guts
{
    public abstract class OperationHandler
    {
        public INerpExcelBuilder Builder { get; }

        protected OperationHandler(INerpExcelBuilder builder)
        {
            Builder = builder;
        }
    }
}