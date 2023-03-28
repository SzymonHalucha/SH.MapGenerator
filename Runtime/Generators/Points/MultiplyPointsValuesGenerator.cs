namespace SH.MapGenerator.Generators.Points
{
    public class MultiplyPointsValuesGenerator : BasePointsValuesGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("MultiplyPointsValues");
    }
}