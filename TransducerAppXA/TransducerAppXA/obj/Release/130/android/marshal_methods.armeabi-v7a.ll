; ModuleID = 'obj\Release\130\android\marshal_methods.armeabi-v7a.ll'
source_filename = "obj\Release\130\android\marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [50 x i32] [
	i32 34715100, ; 0: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 19
	i32 39109920, ; 1: Newtonsoft.Json.dll => 0x254c520 => 5
	i32 230752869, ; 2: Microsoft.CSharp.dll => 0xdc10265 => 2
	i32 321597661, ; 3: System.Numerics => 0x132b30dd => 14
	i32 347068432, ; 4: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 9
	i32 442521989, ; 5: Xamarin.Essentials => 0x1a605985 => 18
	i32 465846621, ; 6: mscorlib => 0x1bc4415d => 4
	i32 469710990, ; 7: System.dll => 0x1bff388e => 13
	i32 690569205, ; 8: System.Xml.Linq.dll => 0x29293ff5 => 23
	i32 748832960, ; 9: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 7
	i32 928116545, ; 10: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 19
	i32 955402788, ; 11: Newtonsoft.Json => 0x38f24a24 => 5
	i32 1098259244, ; 12: System => 0x41761b2c => 13
	i32 1292207520, ; 13: SQLitePCLRaw.core.dll => 0x4d0585a0 => 8
	i32 1411638395, ; 14: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 15
	i32 1592978981, ; 15: System.Runtime.Serialization.dll => 0x5ef2ee25 => 22
	i32 1639515021, ; 16: System.Net.Http.dll => 0x61b9038d => 20
	i32 1711441057, ; 17: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 9
	i32 1776026572, ; 18: System.Core.dll => 0x69dc03cc => 12
	i32 1867746548, ; 19: Xamarin.Essentials.dll => 0x6f538cf4 => 18
	i32 2011961780, ; 20: System.Buffers.dll => 0x77ec19b4 => 11
	i32 2103459038, ; 21: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 10
	i32 2201231467, ; 22: System.Net.Http => 0x8334206b => 20
	i32 2465273461, ; 23: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 7
	i32 2475788418, ; 24: Java.Interop.dll => 0x93918882 => 1
	i32 2562349572, ; 25: Microsoft.CSharp => 0x98ba5a04 => 2
	i32 2819470561, ; 26: System.Xml.dll => 0xa80db4e1 => 16
	i32 2905242038, ; 27: mscorlib.dll => 0xad2a79b6 => 4
	i32 3111772706, ; 28: System.Runtime.Serialization => 0xb979e222 => 22
	i32 3204380047, ; 29: System.Data.dll => 0xbefef58f => 21
	i32 3247949154, ; 30: Mono.Security => 0xc197c562 => 24
	i32 3286872994, ; 31: SQLite-net.dll => 0xc3e9b3a2 => 6
	i32 3317144872, ; 32: System.Data => 0xc5b79d28 => 21
	i32 3336601974, ; 33: TransducerAppXA => 0xc6e08176 => 0
	i32 3360279109, ; 34: SQLitePCLRaw.core => 0xc849ca45 => 8
	i32 3362522851, ; 35: Xamarin.AndroidX.Core => 0xc86c06e3 => 17
	i32 3366347497, ; 36: Java.Interop => 0xc8a662e9 => 1
	i32 3395150330, ; 37: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 15
	i32 3429136800, ; 38: System.Xml => 0xcc6479a0 => 16
	i32 3476120550, ; 39: Mono.Android => 0xcf3163e6 => 3
	i32 3509114376, ; 40: System.Xml.Linq => 0xd128d608 => 23
	i32 3672681054, ; 41: Mono.Android.dll => 0xdae8aa5e => 3
	i32 3754567612, ; 42: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 10
	i32 3829621856, ; 43: System.Numerics.dll => 0xe4436460 => 14
	i32 3876362041, ; 44: SQLite-net => 0xe70c9739 => 6
	i32 3896760992, ; 45: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 17
	i32 4105002889, ; 46: Mono.Security.dll => 0xf4ad5f89 => 24
	i32 4151237749, ; 47: System.Core => 0xf76edc75 => 12
	i32 4219483433, ; 48: TransducerAppXA.dll => 0xfb803529 => 0
	i32 4260525087 ; 49: System.Buffers => 0xfdf2741f => 11
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [50 x i32] [
	i32 19, i32 5, i32 2, i32 14, i32 9, i32 18, i32 4, i32 13, ; 0..7
	i32 23, i32 7, i32 19, i32 5, i32 13, i32 8, i32 15, i32 22, ; 8..15
	i32 20, i32 9, i32 12, i32 18, i32 11, i32 10, i32 20, i32 7, ; 16..23
	i32 1, i32 2, i32 16, i32 4, i32 22, i32 21, i32 24, i32 6, ; 24..31
	i32 21, i32 0, i32 8, i32 17, i32 1, i32 15, i32 16, i32 3, ; 32..39
	i32 23, i32 3, i32 10, i32 14, i32 6, i32 17, i32 24, i32 12, ; 40..47
	i32 0, i32 11 ; 48..49
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="all" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="all" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+thumb-mode,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
