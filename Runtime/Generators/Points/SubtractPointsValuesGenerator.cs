namespace SH.MapGenerator.Generators.Points
{
    public class SubtractPointsValuesGenerator : BasePointsValuesGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("SubtractPointsValues");
    }
}