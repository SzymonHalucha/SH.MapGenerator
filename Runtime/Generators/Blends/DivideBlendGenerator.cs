namespace SH.MapGenerator.Generators.Blends
{
    public class DivideBlendGenerator : BaseBlendGenerator
    {
        public override void Generate(RuntimeData data) => SetupComputeShader("DivideBlend");
    }
}