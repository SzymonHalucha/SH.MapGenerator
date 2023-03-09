namespace SH.MapGenerator.Generators.Blends
{
    public class SubtractBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("SubtractBlend");
    }
}