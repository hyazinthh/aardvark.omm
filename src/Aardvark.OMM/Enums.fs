namespace Aardvark.OMM

open System

type BakerType =
    | GPU = 0
    | CPU = 1

type MessageSeverity =
    | Info        = 0
    | PerfWarning = 1
    | Error       = 2
    | Fatal       = 3

[<Flags>]
type CpuBakeFlags =
   | None = 0u

   /// Baker will use internal threads to run the baking process in parallel.
   | EnableInternalThreads        = 1u

   /// Will disable the use of special indices in case the OMM-state is uniform, Only set this flag for debug purposes.
   /// Note: This prevents promotion of fully known OMMs to use special indices, however for invalid & degenerate UV triangles
   /// special indices may still be set.
   | DisableSpecialIndices        = 2u

   /// Force 32-bit index format for the output OMM index buffer
   | Force32BitIndices            = 4u

   /// Will disable reuse of OMMs and instead produce duplicates omm-array data. Generally only needed for debug purposes.
   | DisableDuplicateDetection    = 8u

   /// This enables merging of "similar" OMMs where similarity is measured using hamming distance.
   /// UT and UO are considered identical.
   /// Pros: normally reduces resulting OMM size drastically, especially when there's overlapping UVs.
   /// Cons: The merging comes at the cost of coverage.
   /// The resulting OMM Arrays will have lower fraction of known states. For large working sets it can be quite CPU heavy to
   /// compute. Note: can not be used if DisableDuplicateDetection is set.
   | EnableNearDuplicateDetection = 16u

   /// Enable additional validation, when enabled additional processing is performed to validate quality and sanity of input data
   /// which may help diagnose omm bake result or longer than expected bake times.
   /// *** NOTE messageInterface must be set when using this flag ***
   | EnableValidation             = 32u

   /// Allow 8-bit index format for the output OMM index buffer
   | Allow8BitIndices             = 64u