namespace SH.MapGenerator.Generators.Blends
{
    public class MaxBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("MaxBlend");
    }
}