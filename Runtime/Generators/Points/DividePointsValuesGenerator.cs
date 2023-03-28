namespace SH.MapGenerator.Generators.Points
{
    public class DividePointsValuesGenerator : BasePointsValuesGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("DividePointsValues");
    }
}