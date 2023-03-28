namespace SH.MapGenerator.Generators.Points
{
    public class AddPointsValuesGenerator : BasePointsValuesGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("AddPointsValues");
    }
}