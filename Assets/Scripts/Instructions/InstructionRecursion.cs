namespace PipelineDreams
{
    public class InstructionRecursion : Instruction {
        public InstructionRecursion(EntityDataContainer eM, Entity player, CommandsContainer pC, InstructionData data, string variant) : base(eM, player, pC, data, variant) {
        }

        public override IClockTask Operation(float startClock)
        {

            return new InstructionRecursionTask();
        }
        
    }
    public abstract partial class Instruction
    {
        /// <summary>
        /// Field instruction task used above.
        /// </summary>
        protected class InstructionRecursionTask : InstructionTask
        {
            protected override void OnRunStart()
            {
            }
        }
    }

}
