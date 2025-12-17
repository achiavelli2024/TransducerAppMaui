; ModuleID = 'obj\Release\130\android\typemaps.armeabi-v7a.ll'
source_filename = "obj\Release\130\android\typemaps.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.TypeMapJava = type {
	i32,; uint32_t module_index
	i32,; uint32_t type_token_id
	i32; uint32_t java_name_index
}

%struct.TypeMapModule = type {
	[16 x i8],; uint8_t module_uuid[16]
	i32,; uint32_t entry_count
	i32,; uint32_t duplicate_count
	%struct.TypeMapModuleEntry*,; TypeMapModuleEntry* map
	%struct.TypeMapModuleEntry*,; TypeMapModuleEntry* duplicate_map
	i8*,; char* assembly_name
	%struct.MonoImage*,; MonoImage* image
	i32,; uint32_t java_name_width
	i8*; uint8_t* java_map
}

%struct.TypeMapModuleEntry = type {
	i32,; uint32_t type_token_id
	i32; uint32_t java_map_index
}

@map_module_count = local_unnamed_addr constant i32 4, align 4

@java_type_count = local_unnamed_addr constant i32 324, align 4

; Map modules data

; module0_managed_to_java
@module0_managed_to_java = internal constant [305 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554576, ; type_token_id
		i32 254; java_map_index
	}, 
	; 1
	%struct.TypeMapModuleEntry {
		i32 33554577, ; type_token_id
		i32 94; java_map_index
	}, 
	; 2
	%struct.TypeMapModuleEntry {
		i32 33554579, ; type_token_id
		i32 21; java_map_index
	}, 
	; 3
	%struct.TypeMapModuleEntry {
		i32 33554581, ; type_token_id
		i32 117; java_map_index
	}, 
	; 4
	%struct.TypeMapModuleEntry {
		i32 33554582, ; type_token_id
		i32 274; java_map_index
	}, 
	; 5
	%struct.TypeMapModuleEntry {
		i32 33554584, ; type_token_id
		i32 252; java_map_index
	}, 
	; 6
	%struct.TypeMapModuleEntry {
		i32 33554586, ; type_token_id
		i32 206; java_map_index
	}, 
	; 7
	%struct.TypeMapModuleEntry {
		i32 33554588, ; type_token_id
		i32 106; java_map_index
	}, 
	; 8
	%struct.TypeMapModuleEntry {
		i32 33554590, ; type_token_id
		i32 246; java_map_index
	}, 
	; 9
	%struct.TypeMapModuleEntry {
		i32 33554592, ; type_token_id
		i32 9; java_map_index
	}, 
	; 10
	%struct.TypeMapModuleEntry {
		i32 33554594, ; type_token_id
		i32 59; java_map_index
	}, 
	; 11
	%struct.TypeMapModuleEntry {
		i32 33554596, ; type_token_id
		i32 281; java_map_index
	}, 
	; 12
	%struct.TypeMapModuleEntry {
		i32 33554598, ; type_token_id
		i32 83; java_map_index
	}, 
	; 13
	%struct.TypeMapModuleEntry {
		i32 33554599, ; type_token_id
		i32 101; java_map_index
	}, 
	; 14
	%struct.TypeMapModuleEntry {
		i32 33554600, ; type_token_id
		i32 295; java_map_index
	}, 
	; 15
	%struct.TypeMapModuleEntry {
		i32 33554602, ; type_token_id
		i32 311; java_map_index
	}, 
	; 16
	%struct.TypeMapModuleEntry {
		i32 33554612, ; type_token_id
		i32 15; java_map_index
	}, 
	; 17
	%struct.TypeMapModuleEntry {
		i32 33554614, ; type_token_id
		i32 195; java_map_index
	}, 
	; 18
	%struct.TypeMapModuleEntry {
		i32 33554616, ; type_token_id
		i32 261; java_map_index
	}, 
	; 19
	%struct.TypeMapModuleEntry {
		i32 33554619, ; type_token_id
		i32 35; java_map_index
	}, 
	; 20
	%struct.TypeMapModuleEntry {
		i32 33554621, ; type_token_id
		i32 168; java_map_index
	}, 
	; 21
	%struct.TypeMapModuleEntry {
		i32 33554622, ; type_token_id
		i32 163; java_map_index
	}, 
	; 22
	%struct.TypeMapModuleEntry {
		i32 33554625, ; type_token_id
		i32 309; java_map_index
	}, 
	; 23
	%struct.TypeMapModuleEntry {
		i32 33554626, ; type_token_id
		i32 157; java_map_index
	}, 
	; 24
	%struct.TypeMapModuleEntry {
		i32 33554630, ; type_token_id
		i32 51; java_map_index
	}, 
	; 25
	%struct.TypeMapModuleEntry {
		i32 33554637, ; type_token_id
		i32 251; java_map_index
	}, 
	; 26
	%struct.TypeMapModuleEntry {
		i32 33554638, ; type_token_id
		i32 201; java_map_index
	}, 
	; 27
	%struct.TypeMapModuleEntry {
		i32 33554640, ; type_token_id
		i32 122; java_map_index
	}, 
	; 28
	%struct.TypeMapModuleEntry {
		i32 33554641, ; type_token_id
		i32 139; java_map_index
	}, 
	; 29
	%struct.TypeMapModuleEntry {
		i32 33554642, ; type_token_id
		i32 323; java_map_index
	}, 
	; 30
	%struct.TypeMapModuleEntry {
		i32 33554645, ; type_token_id
		i32 181; java_map_index
	}, 
	; 31
	%struct.TypeMapModuleEntry {
		i32 33554650, ; type_token_id
		i32 74; java_map_index
	}, 
	; 32
	%struct.TypeMapModuleEntry {
		i32 33554651, ; type_token_id
		i32 44; java_map_index
	}, 
	; 33
	%struct.TypeMapModuleEntry {
		i32 33554652, ; type_token_id
		i32 202; java_map_index
	}, 
	; 34
	%struct.TypeMapModuleEntry {
		i32 33554655, ; type_token_id
		i32 141; java_map_index
	}, 
	; 35
	%struct.TypeMapModuleEntry {
		i32 33554656, ; type_token_id
		i32 166; java_map_index
	}, 
	; 36
	%struct.TypeMapModuleEntry {
		i32 33554657, ; type_token_id
		i32 68; java_map_index
	}, 
	; 37
	%struct.TypeMapModuleEntry {
		i32 33554658, ; type_token_id
		i32 207; java_map_index
	}, 
	; 38
	%struct.TypeMapModuleEntry {
		i32 33554660, ; type_token_id
		i32 211; java_map_index
	}, 
	; 39
	%struct.TypeMapModuleEntry {
		i32 33554662, ; type_token_id
		i32 100; java_map_index
	}, 
	; 40
	%struct.TypeMapModuleEntry {
		i32 33554664, ; type_token_id
		i32 3; java_map_index
	}, 
	; 41
	%struct.TypeMapModuleEntry {
		i32 33554666, ; type_token_id
		i32 172; java_map_index
	}, 
	; 42
	%struct.TypeMapModuleEntry {
		i32 33554668, ; type_token_id
		i32 5; java_map_index
	}, 
	; 43
	%struct.TypeMapModuleEntry {
		i32 33554670, ; type_token_id
		i32 152; java_map_index
	}, 
	; 44
	%struct.TypeMapModuleEntry {
		i32 33554671, ; type_token_id
		i32 113; java_map_index
	}, 
	; 45
	%struct.TypeMapModuleEntry {
		i32 33554672, ; type_token_id
		i32 229; java_map_index
	}, 
	; 46
	%struct.TypeMapModuleEntry {
		i32 33554673, ; type_token_id
		i32 114; java_map_index
	}, 
	; 47
	%struct.TypeMapModuleEntry {
		i32 33554674, ; type_token_id
		i32 87; java_map_index
	}, 
	; 48
	%struct.TypeMapModuleEntry {
		i32 33554675, ; type_token_id
		i32 58; java_map_index
	}, 
	; 49
	%struct.TypeMapModuleEntry {
		i32 33554676, ; type_token_id
		i32 242; java_map_index
	}, 
	; 50
	%struct.TypeMapModuleEntry {
		i32 33554679, ; type_token_id
		i32 248; java_map_index
	}, 
	; 51
	%struct.TypeMapModuleEntry {
		i32 33554683, ; type_token_id
		i32 219; java_map_index
	}, 
	; 52
	%struct.TypeMapModuleEntry {
		i32 33554684, ; type_token_id
		i32 232; java_map_index
	}, 
	; 53
	%struct.TypeMapModuleEntry {
		i32 33554685, ; type_token_id
		i32 189; java_map_index
	}, 
	; 54
	%struct.TypeMapModuleEntry {
		i32 33554686, ; type_token_id
		i32 209; java_map_index
	}, 
	; 55
	%struct.TypeMapModuleEntry {
		i32 33554687, ; type_token_id
		i32 280; java_map_index
	}, 
	; 56
	%struct.TypeMapModuleEntry {
		i32 33554688, ; type_token_id
		i32 293; java_map_index
	}, 
	; 57
	%struct.TypeMapModuleEntry {
		i32 33554696, ; type_token_id
		i32 222; java_map_index
	}, 
	; 58
	%struct.TypeMapModuleEntry {
		i32 33554697, ; type_token_id
		i32 239; java_map_index
	}, 
	; 59
	%struct.TypeMapModuleEntry {
		i32 33554700, ; type_token_id
		i32 267; java_map_index
	}, 
	; 60
	%struct.TypeMapModuleEntry {
		i32 33554702, ; type_token_id
		i32 167; java_map_index
	}, 
	; 61
	%struct.TypeMapModuleEntry {
		i32 33554703, ; type_token_id
		i32 129; java_map_index
	}, 
	; 62
	%struct.TypeMapModuleEntry {
		i32 33554704, ; type_token_id
		i32 220; java_map_index
	}, 
	; 63
	%struct.TypeMapModuleEntry {
		i32 33554705, ; type_token_id
		i32 198; java_map_index
	}, 
	; 64
	%struct.TypeMapModuleEntry {
		i32 33554707, ; type_token_id
		i32 289; java_map_index
	}, 
	; 65
	%struct.TypeMapModuleEntry {
		i32 33554709, ; type_token_id
		i32 319; java_map_index
	}, 
	; 66
	%struct.TypeMapModuleEntry {
		i32 33554711, ; type_token_id
		i32 23; java_map_index
	}, 
	; 67
	%struct.TypeMapModuleEntry {
		i32 33554713, ; type_token_id
		i32 318; java_map_index
	}, 
	; 68
	%struct.TypeMapModuleEntry {
		i32 33554715, ; type_token_id
		i32 245; java_map_index
	}, 
	; 69
	%struct.TypeMapModuleEntry {
		i32 33554717, ; type_token_id
		i32 226; java_map_index
	}, 
	; 70
	%struct.TypeMapModuleEntry {
		i32 33554719, ; type_token_id
		i32 91; java_map_index
	}, 
	; 71
	%struct.TypeMapModuleEntry {
		i32 33554721, ; type_token_id
		i32 112; java_map_index
	}, 
	; 72
	%struct.TypeMapModuleEntry {
		i32 33554723, ; type_token_id
		i32 39; java_map_index
	}, 
	; 73
	%struct.TypeMapModuleEntry {
		i32 33554725, ; type_token_id
		i32 125; java_map_index
	}, 
	; 74
	%struct.TypeMapModuleEntry {
		i32 33554726, ; type_token_id
		i32 241; java_map_index
	}, 
	; 75
	%struct.TypeMapModuleEntry {
		i32 33554728, ; type_token_id
		i32 240; java_map_index
	}, 
	; 76
	%struct.TypeMapModuleEntry {
		i32 33554729, ; type_token_id
		i32 10; java_map_index
	}, 
	; 77
	%struct.TypeMapModuleEntry {
		i32 33554730, ; type_token_id
		i32 71; java_map_index
	}, 
	; 78
	%struct.TypeMapModuleEntry {
		i32 33554732, ; type_token_id
		i32 262; java_map_index
	}, 
	; 79
	%struct.TypeMapModuleEntry {
		i32 33554733, ; type_token_id
		i32 38; java_map_index
	}, 
	; 80
	%struct.TypeMapModuleEntry {
		i32 33554735, ; type_token_id
		i32 283; java_map_index
	}, 
	; 81
	%struct.TypeMapModuleEntry {
		i32 33554738, ; type_token_id
		i32 208; java_map_index
	}, 
	; 82
	%struct.TypeMapModuleEntry {
		i32 33554739, ; type_token_id
		i32 153; java_map_index
	}, 
	; 83
	%struct.TypeMapModuleEntry {
		i32 33554740, ; type_token_id
		i32 135; java_map_index
	}, 
	; 84
	%struct.TypeMapModuleEntry {
		i32 33554741, ; type_token_id
		i32 213; java_map_index
	}, 
	; 85
	%struct.TypeMapModuleEntry {
		i32 33554742, ; type_token_id
		i32 148; java_map_index
	}, 
	; 86
	%struct.TypeMapModuleEntry {
		i32 33554744, ; type_token_id
		i32 306; java_map_index
	}, 
	; 87
	%struct.TypeMapModuleEntry {
		i32 33554745, ; type_token_id
		i32 67; java_map_index
	}, 
	; 88
	%struct.TypeMapModuleEntry {
		i32 33554747, ; type_token_id
		i32 190; java_map_index
	}, 
	; 89
	%struct.TypeMapModuleEntry {
		i32 33554750, ; type_token_id
		i32 244; java_map_index
	}, 
	; 90
	%struct.TypeMapModuleEntry {
		i32 33554756, ; type_token_id
		i32 6; java_map_index
	}, 
	; 91
	%struct.TypeMapModuleEntry {
		i32 33554757, ; type_token_id
		i32 26; java_map_index
	}, 
	; 92
	%struct.TypeMapModuleEntry {
		i32 33554758, ; type_token_id
		i32 37; java_map_index
	}, 
	; 93
	%struct.TypeMapModuleEntry {
		i32 33554760, ; type_token_id
		i32 46; java_map_index
	}, 
	; 94
	%struct.TypeMapModuleEntry {
		i32 33554761, ; type_token_id
		i32 316; java_map_index
	}, 
	; 95
	%struct.TypeMapModuleEntry {
		i32 33554763, ; type_token_id
		i32 271; java_map_index
	}, 
	; 96
	%struct.TypeMapModuleEntry {
		i32 33554765, ; type_token_id
		i32 85; java_map_index
	}, 
	; 97
	%struct.TypeMapModuleEntry {
		i32 33554767, ; type_token_id
		i32 277; java_map_index
	}, 
	; 98
	%struct.TypeMapModuleEntry {
		i32 33554768, ; type_token_id
		i32 95; java_map_index
	}, 
	; 99
	%struct.TypeMapModuleEntry {
		i32 33554771, ; type_token_id
		i32 75; java_map_index
	}, 
	; 100
	%struct.TypeMapModuleEntry {
		i32 33554785, ; type_token_id
		i32 265; java_map_index
	}, 
	; 101
	%struct.TypeMapModuleEntry {
		i32 33554786, ; type_token_id
		i32 165; java_map_index
	}, 
	; 102
	%struct.TypeMapModuleEntry {
		i32 33554787, ; type_token_id
		i32 20; java_map_index
	}, 
	; 103
	%struct.TypeMapModuleEntry {
		i32 33554792, ; type_token_id
		i32 7; java_map_index
	}, 
	; 104
	%struct.TypeMapModuleEntry {
		i32 33554793, ; type_token_id
		i32 227; java_map_index
	}, 
	; 105
	%struct.TypeMapModuleEntry {
		i32 33554795, ; type_token_id
		i32 210; java_map_index
	}, 
	; 106
	%struct.TypeMapModuleEntry {
		i32 33554797, ; type_token_id
		i32 310; java_map_index
	}, 
	; 107
	%struct.TypeMapModuleEntry {
		i32 33554799, ; type_token_id
		i32 249; java_map_index
	}, 
	; 108
	%struct.TypeMapModuleEntry {
		i32 33554802, ; type_token_id
		i32 12; java_map_index
	}, 
	; 109
	%struct.TypeMapModuleEntry {
		i32 33554805, ; type_token_id
		i32 287; java_map_index
	}, 
	; 110
	%struct.TypeMapModuleEntry {
		i32 33554807, ; type_token_id
		i32 273; java_map_index
	}, 
	; 111
	%struct.TypeMapModuleEntry {
		i32 33554809, ; type_token_id
		i32 159; java_map_index
	}, 
	; 112
	%struct.TypeMapModuleEntry {
		i32 33554811, ; type_token_id
		i32 216; java_map_index
	}, 
	; 113
	%struct.TypeMapModuleEntry {
		i32 33554814, ; type_token_id
		i32 147; java_map_index
	}, 
	; 114
	%struct.TypeMapModuleEntry {
		i32 33554817, ; type_token_id
		i32 313; java_map_index
	}, 
	; 115
	%struct.TypeMapModuleEntry {
		i32 33554819, ; type_token_id
		i32 268; java_map_index
	}, 
	; 116
	%struct.TypeMapModuleEntry {
		i32 33554823, ; type_token_id
		i32 24; java_map_index
	}, 
	; 117
	%struct.TypeMapModuleEntry {
		i32 33554825, ; type_token_id
		i32 156; java_map_index
	}, 
	; 118
	%struct.TypeMapModuleEntry {
		i32 33554826, ; type_token_id
		i32 215; java_map_index
	}, 
	; 119
	%struct.TypeMapModuleEntry {
		i32 33554828, ; type_token_id
		i32 19; java_map_index
	}, 
	; 120
	%struct.TypeMapModuleEntry {
		i32 33554829, ; type_token_id
		i32 259; java_map_index
	}, 
	; 121
	%struct.TypeMapModuleEntry {
		i32 33554830, ; type_token_id
		i32 257; java_map_index
	}, 
	; 122
	%struct.TypeMapModuleEntry {
		i32 33554831, ; type_token_id
		i32 84; java_map_index
	}, 
	; 123
	%struct.TypeMapModuleEntry {
		i32 33554832, ; type_token_id
		i32 0; java_map_index
	}, 
	; 124
	%struct.TypeMapModuleEntry {
		i32 33554833, ; type_token_id
		i32 237; java_map_index
	}, 
	; 125
	%struct.TypeMapModuleEntry {
		i32 33554834, ; type_token_id
		i32 143; java_map_index
	}, 
	; 126
	%struct.TypeMapModuleEntry {
		i32 33554835, ; type_token_id
		i32 218; java_map_index
	}, 
	; 127
	%struct.TypeMapModuleEntry {
		i32 33554836, ; type_token_id
		i32 272; java_map_index
	}, 
	; 128
	%struct.TypeMapModuleEntry {
		i32 33554837, ; type_token_id
		i32 47; java_map_index
	}, 
	; 129
	%struct.TypeMapModuleEntry {
		i32 33554838, ; type_token_id
		i32 176; java_map_index
	}, 
	; 130
	%struct.TypeMapModuleEntry {
		i32 33554839, ; type_token_id
		i32 130; java_map_index
	}, 
	; 131
	%struct.TypeMapModuleEntry {
		i32 33554840, ; type_token_id
		i32 317; java_map_index
	}, 
	; 132
	%struct.TypeMapModuleEntry {
		i32 33554842, ; type_token_id
		i32 308; java_map_index
	}, 
	; 133
	%struct.TypeMapModuleEntry {
		i32 33554843, ; type_token_id
		i32 40; java_map_index
	}, 
	; 134
	%struct.TypeMapModuleEntry {
		i32 33554846, ; type_token_id
		i32 187; java_map_index
	}, 
	; 135
	%struct.TypeMapModuleEntry {
		i32 33554847, ; type_token_id
		i32 149; java_map_index
	}, 
	; 136
	%struct.TypeMapModuleEntry {
		i32 33554848, ; type_token_id
		i32 92; java_map_index
	}, 
	; 137
	%struct.TypeMapModuleEntry {
		i32 33554849, ; type_token_id
		i32 290; java_map_index
	}, 
	; 138
	%struct.TypeMapModuleEntry {
		i32 33554850, ; type_token_id
		i32 98; java_map_index
	}, 
	; 139
	%struct.TypeMapModuleEntry {
		i32 33554851, ; type_token_id
		i32 118; java_map_index
	}, 
	; 140
	%struct.TypeMapModuleEntry {
		i32 33554852, ; type_token_id
		i32 320; java_map_index
	}, 
	; 141
	%struct.TypeMapModuleEntry {
		i32 33554854, ; type_token_id
		i32 279; java_map_index
	}, 
	; 142
	%struct.TypeMapModuleEntry {
		i32 33554857, ; type_token_id
		i32 123; java_map_index
	}, 
	; 143
	%struct.TypeMapModuleEntry {
		i32 33554858, ; type_token_id
		i32 89; java_map_index
	}, 
	; 144
	%struct.TypeMapModuleEntry {
		i32 33554859, ; type_token_id
		i32 221; java_map_index
	}, 
	; 145
	%struct.TypeMapModuleEntry {
		i32 33554860, ; type_token_id
		i32 96; java_map_index
	}, 
	; 146
	%struct.TypeMapModuleEntry {
		i32 33554861, ; type_token_id
		i32 225; java_map_index
	}, 
	; 147
	%struct.TypeMapModuleEntry {
		i32 33554862, ; type_token_id
		i32 297; java_map_index
	}, 
	; 148
	%struct.TypeMapModuleEntry {
		i32 33554863, ; type_token_id
		i32 73; java_map_index
	}, 
	; 149
	%struct.TypeMapModuleEntry {
		i32 33554864, ; type_token_id
		i32 134; java_map_index
	}, 
	; 150
	%struct.TypeMapModuleEntry {
		i32 33554865, ; type_token_id
		i32 103; java_map_index
	}, 
	; 151
	%struct.TypeMapModuleEntry {
		i32 33554866, ; type_token_id
		i32 182; java_map_index
	}, 
	; 152
	%struct.TypeMapModuleEntry {
		i32 33554867, ; type_token_id
		i32 269; java_map_index
	}, 
	; 153
	%struct.TypeMapModuleEntry {
		i32 33554868, ; type_token_id
		i32 177; java_map_index
	}, 
	; 154
	%struct.TypeMapModuleEntry {
		i32 33554869, ; type_token_id
		i32 145; java_map_index
	}, 
	; 155
	%struct.TypeMapModuleEntry {
		i32 33554870, ; type_token_id
		i32 69; java_map_index
	}, 
	; 156
	%struct.TypeMapModuleEntry {
		i32 33554871, ; type_token_id
		i32 321; java_map_index
	}, 
	; 157
	%struct.TypeMapModuleEntry {
		i32 33554872, ; type_token_id
		i32 105; java_map_index
	}, 
	; 158
	%struct.TypeMapModuleEntry {
		i32 33554873, ; type_token_id
		i32 292; java_map_index
	}, 
	; 159
	%struct.TypeMapModuleEntry {
		i32 33554874, ; type_token_id
		i32 111; java_map_index
	}, 
	; 160
	%struct.TypeMapModuleEntry {
		i32 33554875, ; type_token_id
		i32 285; java_map_index
	}, 
	; 161
	%struct.TypeMapModuleEntry {
		i32 33554880, ; type_token_id
		i32 212; java_map_index
	}, 
	; 162
	%struct.TypeMapModuleEntry {
		i32 33554881, ; type_token_id
		i32 276; java_map_index
	}, 
	; 163
	%struct.TypeMapModuleEntry {
		i32 33554884, ; type_token_id
		i32 171; java_map_index
	}, 
	; 164
	%struct.TypeMapModuleEntry {
		i32 33554885, ; type_token_id
		i32 80; java_map_index
	}, 
	; 165
	%struct.TypeMapModuleEntry {
		i32 33554887, ; type_token_id
		i32 77; java_map_index
	}, 
	; 166
	%struct.TypeMapModuleEntry {
		i32 33554889, ; type_token_id
		i32 64; java_map_index
	}, 
	; 167
	%struct.TypeMapModuleEntry {
		i32 33554893, ; type_token_id
		i32 161; java_map_index
	}, 
	; 168
	%struct.TypeMapModuleEntry {
		i32 33554894, ; type_token_id
		i32 128; java_map_index
	}, 
	; 169
	%struct.TypeMapModuleEntry {
		i32 33554895, ; type_token_id
		i32 173; java_map_index
	}, 
	; 170
	%struct.TypeMapModuleEntry {
		i32 33554896, ; type_token_id
		i32 185; java_map_index
	}, 
	; 171
	%struct.TypeMapModuleEntry {
		i32 33554897, ; type_token_id
		i32 300; java_map_index
	}, 
	; 172
	%struct.TypeMapModuleEntry {
		i32 33554905, ; type_token_id
		i32 178; java_map_index
	}, 
	; 173
	%struct.TypeMapModuleEntry {
		i32 33554906, ; type_token_id
		i32 144; java_map_index
	}, 
	; 174
	%struct.TypeMapModuleEntry {
		i32 33554907, ; type_token_id
		i32 107; java_map_index
	}, 
	; 175
	%struct.TypeMapModuleEntry {
		i32 33554908, ; type_token_id
		i32 126; java_map_index
	}, 
	; 176
	%struct.TypeMapModuleEntry {
		i32 33554910, ; type_token_id
		i32 61; java_map_index
	}, 
	; 177
	%struct.TypeMapModuleEntry {
		i32 33554912, ; type_token_id
		i32 314; java_map_index
	}, 
	; 178
	%struct.TypeMapModuleEntry {
		i32 33554913, ; type_token_id
		i32 180; java_map_index
	}, 
	; 179
	%struct.TypeMapModuleEntry {
		i32 33554914, ; type_token_id
		i32 302; java_map_index
	}, 
	; 180
	%struct.TypeMapModuleEntry {
		i32 33554916, ; type_token_id
		i32 296; java_map_index
	}, 
	; 181
	%struct.TypeMapModuleEntry {
		i32 33554917, ; type_token_id
		i32 194; java_map_index
	}, 
	; 182
	%struct.TypeMapModuleEntry {
		i32 33554919, ; type_token_id
		i32 301; java_map_index
	}, 
	; 183
	%struct.TypeMapModuleEntry {
		i32 33554921, ; type_token_id
		i32 224; java_map_index
	}, 
	; 184
	%struct.TypeMapModuleEntry {
		i32 33554924, ; type_token_id
		i32 291; java_map_index
	}, 
	; 185
	%struct.TypeMapModuleEntry {
		i32 33554925, ; type_token_id
		i32 42; java_map_index
	}, 
	; 186
	%struct.TypeMapModuleEntry {
		i32 33554927, ; type_token_id
		i32 160; java_map_index
	}, 
	; 187
	%struct.TypeMapModuleEntry {
		i32 33554928, ; type_token_id
		i32 169; java_map_index
	}, 
	; 188
	%struct.TypeMapModuleEntry {
		i32 33554930, ; type_token_id
		i32 164; java_map_index
	}, 
	; 189
	%struct.TypeMapModuleEntry {
		i32 33554931, ; type_token_id
		i32 174; java_map_index
	}, 
	; 190
	%struct.TypeMapModuleEntry {
		i32 33554932, ; type_token_id
		i32 22; java_map_index
	}, 
	; 191
	%struct.TypeMapModuleEntry {
		i32 33554934, ; type_token_id
		i32 197; java_map_index
	}, 
	; 192
	%struct.TypeMapModuleEntry {
		i32 33554936, ; type_token_id
		i32 255; java_map_index
	}, 
	; 193
	%struct.TypeMapModuleEntry {
		i32 33554941, ; type_token_id
		i32 146; java_map_index
	}, 
	; 194
	%struct.TypeMapModuleEntry {
		i32 33554942, ; type_token_id
		i32 179; java_map_index
	}, 
	; 195
	%struct.TypeMapModuleEntry {
		i32 33554943, ; type_token_id
		i32 78; java_map_index
	}, 
	; 196
	%struct.TypeMapModuleEntry {
		i32 33554944, ; type_token_id
		i32 192; java_map_index
	}, 
	; 197
	%struct.TypeMapModuleEntry {
		i32 33554945, ; type_token_id
		i32 223; java_map_index
	}, 
	; 198
	%struct.TypeMapModuleEntry {
		i32 33554946, ; type_token_id
		i32 203; java_map_index
	}, 
	; 199
	%struct.TypeMapModuleEntry {
		i32 33554972, ; type_token_id
		i32 294; java_map_index
	}, 
	; 200
	%struct.TypeMapModuleEntry {
		i32 33554975, ; type_token_id
		i32 230; java_map_index
	}, 
	; 201
	%struct.TypeMapModuleEntry {
		i32 33554977, ; type_token_id
		i32 102; java_map_index
	}, 
	; 202
	%struct.TypeMapModuleEntry {
		i32 33554979, ; type_token_id
		i32 116; java_map_index
	}, 
	; 203
	%struct.TypeMapModuleEntry {
		i32 33554988, ; type_token_id
		i32 52; java_map_index
	}, 
	; 204
	%struct.TypeMapModuleEntry {
		i32 33554990, ; type_token_id
		i32 266; java_map_index
	}, 
	; 205
	%struct.TypeMapModuleEntry {
		i32 33554992, ; type_token_id
		i32 258; java_map_index
	}, 
	; 206
	%struct.TypeMapModuleEntry {
		i32 33554993, ; type_token_id
		i32 315; java_map_index
	}, 
	; 207
	%struct.TypeMapModuleEntry {
		i32 33555009, ; type_token_id
		i32 41; java_map_index
	}, 
	; 208
	%struct.TypeMapModuleEntry {
		i32 33555020, ; type_token_id
		i32 278; java_map_index
	}, 
	; 209
	%struct.TypeMapModuleEntry {
		i32 33555021, ; type_token_id
		i32 86; java_map_index
	}, 
	; 210
	%struct.TypeMapModuleEntry {
		i32 33555023, ; type_token_id
		i32 31; java_map_index
	}, 
	; 211
	%struct.TypeMapModuleEntry {
		i32 33555025, ; type_token_id
		i32 142; java_map_index
	}, 
	; 212
	%struct.TypeMapModuleEntry {
		i32 33555026, ; type_token_id
		i32 82; java_map_index
	}, 
	; 213
	%struct.TypeMapModuleEntry {
		i32 33555028, ; type_token_id
		i32 121; java_map_index
	}, 
	; 214
	%struct.TypeMapModuleEntry {
		i32 33555030, ; type_token_id
		i32 236; java_map_index
	}, 
	; 215
	%struct.TypeMapModuleEntry {
		i32 33555032, ; type_token_id
		i32 36; java_map_index
	}, 
	; 216
	%struct.TypeMapModuleEntry {
		i32 33555033, ; type_token_id
		i32 133; java_map_index
	}, 
	; 217
	%struct.TypeMapModuleEntry {
		i32 33555035, ; type_token_id
		i32 288; java_map_index
	}, 
	; 218
	%struct.TypeMapModuleEntry {
		i32 33555036, ; type_token_id
		i32 29; java_map_index
	}, 
	; 219
	%struct.TypeMapModuleEntry {
		i32 33555038, ; type_token_id
		i32 79; java_map_index
	}, 
	; 220
	%struct.TypeMapModuleEntry {
		i32 33555040, ; type_token_id
		i32 175; java_map_index
	}, 
	; 221
	%struct.TypeMapModuleEntry {
		i32 33555041, ; type_token_id
		i32 4; java_map_index
	}, 
	; 222
	%struct.TypeMapModuleEntry {
		i32 33555043, ; type_token_id
		i32 150; java_map_index
	}, 
	; 223
	%struct.TypeMapModuleEntry {
		i32 33555044, ; type_token_id
		i32 170; java_map_index
	}, 
	; 224
	%struct.TypeMapModuleEntry {
		i32 33555046, ; type_token_id
		i32 200; java_map_index
	}, 
	; 225
	%struct.TypeMapModuleEntry {
		i32 33555048, ; type_token_id
		i32 305; java_map_index
	}, 
	; 226
	%struct.TypeMapModuleEntry {
		i32 33555050, ; type_token_id
		i32 66; java_map_index
	}, 
	; 227
	%struct.TypeMapModuleEntry {
		i32 33555052, ; type_token_id
		i32 43; java_map_index
	}, 
	; 228
	%struct.TypeMapModuleEntry {
		i32 33555054, ; type_token_id
		i32 131; java_map_index
	}, 
	; 229
	%struct.TypeMapModuleEntry {
		i32 33555056, ; type_token_id
		i32 16; java_map_index
	}, 
	; 230
	%struct.TypeMapModuleEntry {
		i32 33555058, ; type_token_id
		i32 72; java_map_index
	}, 
	; 231
	%struct.TypeMapModuleEntry {
		i32 33555060, ; type_token_id
		i32 154; java_map_index
	}, 
	; 232
	%struct.TypeMapModuleEntry {
		i32 33555062, ; type_token_id
		i32 196; java_map_index
	}, 
	; 233
	%struct.TypeMapModuleEntry {
		i32 33555064, ; type_token_id
		i32 132; java_map_index
	}, 
	; 234
	%struct.TypeMapModuleEntry {
		i32 33555066, ; type_token_id
		i32 8; java_map_index
	}, 
	; 235
	%struct.TypeMapModuleEntry {
		i32 33555068, ; type_token_id
		i32 17; java_map_index
	}, 
	; 236
	%struct.TypeMapModuleEntry {
		i32 33555070, ; type_token_id
		i32 120; java_map_index
	}, 
	; 237
	%struct.TypeMapModuleEntry {
		i32 33555072, ; type_token_id
		i32 32; java_map_index
	}, 
	; 238
	%struct.TypeMapModuleEntry {
		i32 33555073, ; type_token_id
		i32 110; java_map_index
	}, 
	; 239
	%struct.TypeMapModuleEntry {
		i32 33555075, ; type_token_id
		i32 264; java_map_index
	}, 
	; 240
	%struct.TypeMapModuleEntry {
		i32 33555076, ; type_token_id
		i32 275; java_map_index
	}, 
	; 241
	%struct.TypeMapModuleEntry {
		i32 33555077, ; type_token_id
		i32 247; java_map_index
	}, 
	; 242
	%struct.TypeMapModuleEntry {
		i32 33555078, ; type_token_id
		i32 184; java_map_index
	}, 
	; 243
	%struct.TypeMapModuleEntry {
		i32 33555079, ; type_token_id
		i32 214; java_map_index
	}, 
	; 244
	%struct.TypeMapModuleEntry {
		i32 33555081, ; type_token_id
		i32 76; java_map_index
	}, 
	; 245
	%struct.TypeMapModuleEntry {
		i32 33555083, ; type_token_id
		i32 193; java_map_index
	}, 
	; 246
	%struct.TypeMapModuleEntry {
		i32 33555084, ; type_token_id
		i32 97; java_map_index
	}, 
	; 247
	%struct.TypeMapModuleEntry {
		i32 33555085, ; type_token_id
		i32 183; java_map_index
	}, 
	; 248
	%struct.TypeMapModuleEntry {
		i32 33555086, ; type_token_id
		i32 25; java_map_index
	}, 
	; 249
	%struct.TypeMapModuleEntry {
		i32 33555087, ; type_token_id
		i32 49; java_map_index
	}, 
	; 250
	%struct.TypeMapModuleEntry {
		i32 33555088, ; type_token_id
		i32 256; java_map_index
	}, 
	; 251
	%struct.TypeMapModuleEntry {
		i32 33555091, ; type_token_id
		i32 162; java_map_index
	}, 
	; 252
	%struct.TypeMapModuleEntry {
		i32 33555092, ; type_token_id
		i32 138; java_map_index
	}, 
	; 253
	%struct.TypeMapModuleEntry {
		i32 33555093, ; type_token_id
		i32 140; java_map_index
	}, 
	; 254
	%struct.TypeMapModuleEntry {
		i32 33555094, ; type_token_id
		i32 282; java_map_index
	}, 
	; 255
	%struct.TypeMapModuleEntry {
		i32 33555095, ; type_token_id
		i32 104; java_map_index
	}, 
	; 256
	%struct.TypeMapModuleEntry {
		i32 33555096, ; type_token_id
		i32 90; java_map_index
	}, 
	; 257
	%struct.TypeMapModuleEntry {
		i32 33555097, ; type_token_id
		i32 270; java_map_index
	}, 
	; 258
	%struct.TypeMapModuleEntry {
		i32 33555098, ; type_token_id
		i32 188; java_map_index
	}, 
	; 259
	%struct.TypeMapModuleEntry {
		i32 33555100, ; type_token_id
		i32 298; java_map_index
	}, 
	; 260
	%struct.TypeMapModuleEntry {
		i32 33555101, ; type_token_id
		i32 228; java_map_index
	}, 
	; 261
	%struct.TypeMapModuleEntry {
		i32 33555102, ; type_token_id
		i32 217; java_map_index
	}, 
	; 262
	%struct.TypeMapModuleEntry {
		i32 33555103, ; type_token_id
		i32 93; java_map_index
	}, 
	; 263
	%struct.TypeMapModuleEntry {
		i32 33555105, ; type_token_id
		i32 53; java_map_index
	}, 
	; 264
	%struct.TypeMapModuleEntry {
		i32 33555108, ; type_token_id
		i32 88; java_map_index
	}, 
	; 265
	%struct.TypeMapModuleEntry {
		i32 33555110, ; type_token_id
		i32 108; java_map_index
	}, 
	; 266
	%struct.TypeMapModuleEntry {
		i32 33555112, ; type_token_id
		i32 14; java_map_index
	}, 
	; 267
	%struct.TypeMapModuleEntry {
		i32 33555113, ; type_token_id
		i32 45; java_map_index
	}, 
	; 268
	%struct.TypeMapModuleEntry {
		i32 33555114, ; type_token_id
		i32 137; java_map_index
	}, 
	; 269
	%struct.TypeMapModuleEntry {
		i32 33555115, ; type_token_id
		i32 55; java_map_index
	}, 
	; 270
	%struct.TypeMapModuleEntry {
		i32 33555116, ; type_token_id
		i32 151; java_map_index
	}, 
	; 271
	%struct.TypeMapModuleEntry {
		i32 33555118, ; type_token_id
		i32 33; java_map_index
	}, 
	; 272
	%struct.TypeMapModuleEntry {
		i32 33555119, ; type_token_id
		i32 30; java_map_index
	}, 
	; 273
	%struct.TypeMapModuleEntry {
		i32 33555120, ; type_token_id
		i32 303; java_map_index
	}, 
	; 274
	%struct.TypeMapModuleEntry {
		i32 33555121, ; type_token_id
		i32 57; java_map_index
	}, 
	; 275
	%struct.TypeMapModuleEntry {
		i32 33555122, ; type_token_id
		i32 253; java_map_index
	}, 
	; 276
	%struct.TypeMapModuleEntry {
		i32 33555124, ; type_token_id
		i32 1; java_map_index
	}, 
	; 277
	%struct.TypeMapModuleEntry {
		i32 33555125, ; type_token_id
		i32 18; java_map_index
	}, 
	; 278
	%struct.TypeMapModuleEntry {
		i32 33555126, ; type_token_id
		i32 136; java_map_index
	}, 
	; 279
	%struct.TypeMapModuleEntry {
		i32 33555127, ; type_token_id
		i32 109; java_map_index
	}, 
	; 280
	%struct.TypeMapModuleEntry {
		i32 33555128, ; type_token_id
		i32 62; java_map_index
	}, 
	; 281
	%struct.TypeMapModuleEntry {
		i32 33555129, ; type_token_id
		i32 263; java_map_index
	}, 
	; 282
	%struct.TypeMapModuleEntry {
		i32 33555131, ; type_token_id
		i32 56; java_map_index
	}, 
	; 283
	%struct.TypeMapModuleEntry {
		i32 33555132, ; type_token_id
		i32 27; java_map_index
	}, 
	; 284
	%struct.TypeMapModuleEntry {
		i32 33555133, ; type_token_id
		i32 99; java_map_index
	}, 
	; 285
	%struct.TypeMapModuleEntry {
		i32 33555134, ; type_token_id
		i32 191; java_map_index
	}, 
	; 286
	%struct.TypeMapModuleEntry {
		i32 33555135, ; type_token_id
		i32 205; java_map_index
	}, 
	; 287
	%struct.TypeMapModuleEntry {
		i32 33555136, ; type_token_id
		i32 34; java_map_index
	}, 
	; 288
	%struct.TypeMapModuleEntry {
		i32 33555138, ; type_token_id
		i32 2; java_map_index
	}, 
	; 289
	%struct.TypeMapModuleEntry {
		i32 33555140, ; type_token_id
		i32 186; java_map_index
	}, 
	; 290
	%struct.TypeMapModuleEntry {
		i32 33555142, ; type_token_id
		i32 13; java_map_index
	}, 
	; 291
	%struct.TypeMapModuleEntry {
		i32 33555144, ; type_token_id
		i32 81; java_map_index
	}, 
	; 292
	%struct.TypeMapModuleEntry {
		i32 33555146, ; type_token_id
		i32 124; java_map_index
	}, 
	; 293
	%struct.TypeMapModuleEntry {
		i32 33555147, ; type_token_id
		i32 199; java_map_index
	}, 
	; 294
	%struct.TypeMapModuleEntry {
		i32 33555148, ; type_token_id
		i32 322; java_map_index
	}, 
	; 295
	%struct.TypeMapModuleEntry {
		i32 33555150, ; type_token_id
		i32 243; java_map_index
	}, 
	; 296
	%struct.TypeMapModuleEntry {
		i32 33555152, ; type_token_id
		i32 286; java_map_index
	}, 
	; 297
	%struct.TypeMapModuleEntry {
		i32 33555154, ; type_token_id
		i32 204; java_map_index
	}, 
	; 298
	%struct.TypeMapModuleEntry {
		i32 33555155, ; type_token_id
		i32 312; java_map_index
	}, 
	; 299
	%struct.TypeMapModuleEntry {
		i32 33555156, ; type_token_id
		i32 48; java_map_index
	}, 
	; 300
	%struct.TypeMapModuleEntry {
		i32 33555158, ; type_token_id
		i32 158; java_map_index
	}, 
	; 301
	%struct.TypeMapModuleEntry {
		i32 33555160, ; type_token_id
		i32 54; java_map_index
	}, 
	; 302
	%struct.TypeMapModuleEntry {
		i32 33555161, ; type_token_id
		i32 234; java_map_index
	}, 
	; 303
	%struct.TypeMapModuleEntry {
		i32 33555162, ; type_token_id
		i32 307; java_map_index
	}, 
	; 304
	%struct.TypeMapModuleEntry {
		i32 33555187, ; type_token_id
		i32 231; java_map_index
	}
], align 4; end of 'module0_managed_to_java' array


; module0_managed_to_java_duplicates
@module0_managed_to_java_duplicates = internal constant [142 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554578, ; type_token_id
		i32 94; java_map_index
	}, 
	; 1
	%struct.TypeMapModuleEntry {
		i32 33554580, ; type_token_id
		i32 21; java_map_index
	}, 
	; 2
	%struct.TypeMapModuleEntry {
		i32 33554583, ; type_token_id
		i32 274; java_map_index
	}, 
	; 3
	%struct.TypeMapModuleEntry {
		i32 33554585, ; type_token_id
		i32 252; java_map_index
	}, 
	; 4
	%struct.TypeMapModuleEntry {
		i32 33554587, ; type_token_id
		i32 206; java_map_index
	}, 
	; 5
	%struct.TypeMapModuleEntry {
		i32 33554589, ; type_token_id
		i32 106; java_map_index
	}, 
	; 6
	%struct.TypeMapModuleEntry {
		i32 33554591, ; type_token_id
		i32 246; java_map_index
	}, 
	; 7
	%struct.TypeMapModuleEntry {
		i32 33554593, ; type_token_id
		i32 9; java_map_index
	}, 
	; 8
	%struct.TypeMapModuleEntry {
		i32 33554595, ; type_token_id
		i32 59; java_map_index
	}, 
	; 9
	%struct.TypeMapModuleEntry {
		i32 33554597, ; type_token_id
		i32 281; java_map_index
	}, 
	; 10
	%struct.TypeMapModuleEntry {
		i32 33554601, ; type_token_id
		i32 295; java_map_index
	}, 
	; 11
	%struct.TypeMapModuleEntry {
		i32 33554613, ; type_token_id
		i32 15; java_map_index
	}, 
	; 12
	%struct.TypeMapModuleEntry {
		i32 33554615, ; type_token_id
		i32 195; java_map_index
	}, 
	; 13
	%struct.TypeMapModuleEntry {
		i32 33554618, ; type_token_id
		i32 261; java_map_index
	}, 
	; 14
	%struct.TypeMapModuleEntry {
		i32 33554620, ; type_token_id
		i32 35; java_map_index
	}, 
	; 15
	%struct.TypeMapModuleEntry {
		i32 33554623, ; type_token_id
		i32 163; java_map_index
	}, 
	; 16
	%struct.TypeMapModuleEntry {
		i32 33554627, ; type_token_id
		i32 157; java_map_index
	}, 
	; 17
	%struct.TypeMapModuleEntry {
		i32 33554636, ; type_token_id
		i32 168; java_map_index
	}, 
	; 18
	%struct.TypeMapModuleEntry {
		i32 33554639, ; type_token_id
		i32 201; java_map_index
	}, 
	; 19
	%struct.TypeMapModuleEntry {
		i32 33554643, ; type_token_id
		i32 323; java_map_index
	}, 
	; 20
	%struct.TypeMapModuleEntry {
		i32 33554649, ; type_token_id
		i32 139; java_map_index
	}, 
	; 21
	%struct.TypeMapModuleEntry {
		i32 33554653, ; type_token_id
		i32 202; java_map_index
	}, 
	; 22
	%struct.TypeMapModuleEntry {
		i32 33554654, ; type_token_id
		i32 44; java_map_index
	}, 
	; 23
	%struct.TypeMapModuleEntry {
		i32 33554659, ; type_token_id
		i32 207; java_map_index
	}, 
	; 24
	%struct.TypeMapModuleEntry {
		i32 33554661, ; type_token_id
		i32 211; java_map_index
	}, 
	; 25
	%struct.TypeMapModuleEntry {
		i32 33554663, ; type_token_id
		i32 100; java_map_index
	}, 
	; 26
	%struct.TypeMapModuleEntry {
		i32 33554665, ; type_token_id
		i32 3; java_map_index
	}, 
	; 27
	%struct.TypeMapModuleEntry {
		i32 33554667, ; type_token_id
		i32 172; java_map_index
	}, 
	; 28
	%struct.TypeMapModuleEntry {
		i32 33554669, ; type_token_id
		i32 5; java_map_index
	}, 
	; 29
	%struct.TypeMapModuleEntry {
		i32 33554677, ; type_token_id
		i32 242; java_map_index
	}, 
	; 30
	%struct.TypeMapModuleEntry {
		i32 33554692, ; type_token_id
		i32 168; java_map_index
	}, 
	; 31
	%struct.TypeMapModuleEntry {
		i32 33554693, ; type_token_id
		i32 251; java_map_index
	}, 
	; 32
	%struct.TypeMapModuleEntry {
		i32 33554695, ; type_token_id
		i32 201; java_map_index
	}, 
	; 33
	%struct.TypeMapModuleEntry {
		i32 33554698, ; type_token_id
		i32 239; java_map_index
	}, 
	; 34
	%struct.TypeMapModuleEntry {
		i32 33554699, ; type_token_id
		i32 222; java_map_index
	}, 
	; 35
	%struct.TypeMapModuleEntry {
		i32 33554701, ; type_token_id
		i32 267; java_map_index
	}, 
	; 36
	%struct.TypeMapModuleEntry {
		i32 33554706, ; type_token_id
		i32 198; java_map_index
	}, 
	; 37
	%struct.TypeMapModuleEntry {
		i32 33554708, ; type_token_id
		i32 289; java_map_index
	}, 
	; 38
	%struct.TypeMapModuleEntry {
		i32 33554710, ; type_token_id
		i32 319; java_map_index
	}, 
	; 39
	%struct.TypeMapModuleEntry {
		i32 33554712, ; type_token_id
		i32 23; java_map_index
	}, 
	; 40
	%struct.TypeMapModuleEntry {
		i32 33554714, ; type_token_id
		i32 318; java_map_index
	}, 
	; 41
	%struct.TypeMapModuleEntry {
		i32 33554716, ; type_token_id
		i32 245; java_map_index
	}, 
	; 42
	%struct.TypeMapModuleEntry {
		i32 33554718, ; type_token_id
		i32 226; java_map_index
	}, 
	; 43
	%struct.TypeMapModuleEntry {
		i32 33554720, ; type_token_id
		i32 91; java_map_index
	}, 
	; 44
	%struct.TypeMapModuleEntry {
		i32 33554722, ; type_token_id
		i32 112; java_map_index
	}, 
	; 45
	%struct.TypeMapModuleEntry {
		i32 33554724, ; type_token_id
		i32 39; java_map_index
	}, 
	; 46
	%struct.TypeMapModuleEntry {
		i32 33554727, ; type_token_id
		i32 241; java_map_index
	}, 
	; 47
	%struct.TypeMapModuleEntry {
		i32 33554731, ; type_token_id
		i32 71; java_map_index
	}, 
	; 48
	%struct.TypeMapModuleEntry {
		i32 33554734, ; type_token_id
		i32 38; java_map_index
	}, 
	; 49
	%struct.TypeMapModuleEntry {
		i32 33554736, ; type_token_id
		i32 283; java_map_index
	}, 
	; 50
	%struct.TypeMapModuleEntry {
		i32 33554737, ; type_token_id
		i32 262; java_map_index
	}, 
	; 51
	%struct.TypeMapModuleEntry {
		i32 33554743, ; type_token_id
		i32 148; java_map_index
	}, 
	; 52
	%struct.TypeMapModuleEntry {
		i32 33554746, ; type_token_id
		i32 67; java_map_index
	}, 
	; 53
	%struct.TypeMapModuleEntry {
		i32 33554748, ; type_token_id
		i32 190; java_map_index
	}, 
	; 54
	%struct.TypeMapModuleEntry {
		i32 33554759, ; type_token_id
		i32 6; java_map_index
	}, 
	; 55
	%struct.TypeMapModuleEntry {
		i32 33554762, ; type_token_id
		i32 316; java_map_index
	}, 
	; 56
	%struct.TypeMapModuleEntry {
		i32 33554764, ; type_token_id
		i32 271; java_map_index
	}, 
	; 57
	%struct.TypeMapModuleEntry {
		i32 33554766, ; type_token_id
		i32 85; java_map_index
	}, 
	; 58
	%struct.TypeMapModuleEntry {
		i32 33554769, ; type_token_id
		i32 95; java_map_index
	}, 
	; 59
	%struct.TypeMapModuleEntry {
		i32 33554770, ; type_token_id
		i32 277; java_map_index
	}, 
	; 60
	%struct.TypeMapModuleEntry {
		i32 33554788, ; type_token_id
		i32 20; java_map_index
	}, 
	; 61
	%struct.TypeMapModuleEntry {
		i32 33554794, ; type_token_id
		i32 227; java_map_index
	}, 
	; 62
	%struct.TypeMapModuleEntry {
		i32 33554798, ; type_token_id
		i32 310; java_map_index
	}, 
	; 63
	%struct.TypeMapModuleEntry {
		i32 33554800, ; type_token_id
		i32 249; java_map_index
	}, 
	; 64
	%struct.TypeMapModuleEntry {
		i32 33554803, ; type_token_id
		i32 12; java_map_index
	}, 
	; 65
	%struct.TypeMapModuleEntry {
		i32 33554806, ; type_token_id
		i32 287; java_map_index
	}, 
	; 66
	%struct.TypeMapModuleEntry {
		i32 33554808, ; type_token_id
		i32 273; java_map_index
	}, 
	; 67
	%struct.TypeMapModuleEntry {
		i32 33554810, ; type_token_id
		i32 159; java_map_index
	}, 
	; 68
	%struct.TypeMapModuleEntry {
		i32 33554812, ; type_token_id
		i32 216; java_map_index
	}, 
	; 69
	%struct.TypeMapModuleEntry {
		i32 33554815, ; type_token_id
		i32 147; java_map_index
	}, 
	; 70
	%struct.TypeMapModuleEntry {
		i32 33554818, ; type_token_id
		i32 313; java_map_index
	}, 
	; 71
	%struct.TypeMapModuleEntry {
		i32 33554824, ; type_token_id
		i32 24; java_map_index
	}, 
	; 72
	%struct.TypeMapModuleEntry {
		i32 33554827, ; type_token_id
		i32 215; java_map_index
	}, 
	; 73
	%struct.TypeMapModuleEntry {
		i32 33554841, ; type_token_id
		i32 317; java_map_index
	}, 
	; 74
	%struct.TypeMapModuleEntry {
		i32 33554853, ; type_token_id
		i32 320; java_map_index
	}, 
	; 75
	%struct.TypeMapModuleEntry {
		i32 33554855, ; type_token_id
		i32 279; java_map_index
	}, 
	; 76
	%struct.TypeMapModuleEntry {
		i32 33554882, ; type_token_id
		i32 276; java_map_index
	}, 
	; 77
	%struct.TypeMapModuleEntry {
		i32 33554883, ; type_token_id
		i32 212; java_map_index
	}, 
	; 78
	%struct.TypeMapModuleEntry {
		i32 33554886, ; type_token_id
		i32 80; java_map_index
	}, 
	; 79
	%struct.TypeMapModuleEntry {
		i32 33554888, ; type_token_id
		i32 77; java_map_index
	}, 
	; 80
	%struct.TypeMapModuleEntry {
		i32 33554890, ; type_token_id
		i32 64; java_map_index
	}, 
	; 81
	%struct.TypeMapModuleEntry {
		i32 33554909, ; type_token_id
		i32 126; java_map_index
	}, 
	; 82
	%struct.TypeMapModuleEntry {
		i32 33554911, ; type_token_id
		i32 61; java_map_index
	}, 
	; 83
	%struct.TypeMapModuleEntry {
		i32 33554915, ; type_token_id
		i32 302; java_map_index
	}, 
	; 84
	%struct.TypeMapModuleEntry {
		i32 33554918, ; type_token_id
		i32 194; java_map_index
	}, 
	; 85
	%struct.TypeMapModuleEntry {
		i32 33554920, ; type_token_id
		i32 301; java_map_index
	}, 
	; 86
	%struct.TypeMapModuleEntry {
		i32 33554922, ; type_token_id
		i32 224; java_map_index
	}, 
	; 87
	%struct.TypeMapModuleEntry {
		i32 33554926, ; type_token_id
		i32 42; java_map_index
	}, 
	; 88
	%struct.TypeMapModuleEntry {
		i32 33554929, ; type_token_id
		i32 169; java_map_index
	}, 
	; 89
	%struct.TypeMapModuleEntry {
		i32 33554933, ; type_token_id
		i32 22; java_map_index
	}, 
	; 90
	%struct.TypeMapModuleEntry {
		i32 33554935, ; type_token_id
		i32 197; java_map_index
	}, 
	; 91
	%struct.TypeMapModuleEntry {
		i32 33554937, ; type_token_id
		i32 255; java_map_index
	}, 
	; 92
	%struct.TypeMapModuleEntry {
		i32 33554947, ; type_token_id
		i32 203; java_map_index
	}, 
	; 93
	%struct.TypeMapModuleEntry {
		i32 33554978, ; type_token_id
		i32 102; java_map_index
	}, 
	; 94
	%struct.TypeMapModuleEntry {
		i32 33554984, ; type_token_id
		i32 116; java_map_index
	}, 
	; 95
	%struct.TypeMapModuleEntry {
		i32 33554989, ; type_token_id
		i32 52; java_map_index
	}, 
	; 96
	%struct.TypeMapModuleEntry {
		i32 33554994, ; type_token_id
		i32 315; java_map_index
	}, 
	; 97
	%struct.TypeMapModuleEntry {
		i32 33555022, ; type_token_id
		i32 86; java_map_index
	}, 
	; 98
	%struct.TypeMapModuleEntry {
		i32 33555024, ; type_token_id
		i32 31; java_map_index
	}, 
	; 99
	%struct.TypeMapModuleEntry {
		i32 33555027, ; type_token_id
		i32 82; java_map_index
	}, 
	; 100
	%struct.TypeMapModuleEntry {
		i32 33555029, ; type_token_id
		i32 121; java_map_index
	}, 
	; 101
	%struct.TypeMapModuleEntry {
		i32 33555031, ; type_token_id
		i32 236; java_map_index
	}, 
	; 102
	%struct.TypeMapModuleEntry {
		i32 33555034, ; type_token_id
		i32 133; java_map_index
	}, 
	; 103
	%struct.TypeMapModuleEntry {
		i32 33555037, ; type_token_id
		i32 29; java_map_index
	}, 
	; 104
	%struct.TypeMapModuleEntry {
		i32 33555039, ; type_token_id
		i32 79; java_map_index
	}, 
	; 105
	%struct.TypeMapModuleEntry {
		i32 33555042, ; type_token_id
		i32 4; java_map_index
	}, 
	; 106
	%struct.TypeMapModuleEntry {
		i32 33555045, ; type_token_id
		i32 170; java_map_index
	}, 
	; 107
	%struct.TypeMapModuleEntry {
		i32 33555047, ; type_token_id
		i32 200; java_map_index
	}, 
	; 108
	%struct.TypeMapModuleEntry {
		i32 33555049, ; type_token_id
		i32 305; java_map_index
	}, 
	; 109
	%struct.TypeMapModuleEntry {
		i32 33555051, ; type_token_id
		i32 66; java_map_index
	}, 
	; 110
	%struct.TypeMapModuleEntry {
		i32 33555053, ; type_token_id
		i32 43; java_map_index
	}, 
	; 111
	%struct.TypeMapModuleEntry {
		i32 33555055, ; type_token_id
		i32 131; java_map_index
	}, 
	; 112
	%struct.TypeMapModuleEntry {
		i32 33555057, ; type_token_id
		i32 16; java_map_index
	}, 
	; 113
	%struct.TypeMapModuleEntry {
		i32 33555059, ; type_token_id
		i32 72; java_map_index
	}, 
	; 114
	%struct.TypeMapModuleEntry {
		i32 33555061, ; type_token_id
		i32 154; java_map_index
	}, 
	; 115
	%struct.TypeMapModuleEntry {
		i32 33555063, ; type_token_id
		i32 196; java_map_index
	}, 
	; 116
	%struct.TypeMapModuleEntry {
		i32 33555065, ; type_token_id
		i32 132; java_map_index
	}, 
	; 117
	%struct.TypeMapModuleEntry {
		i32 33555067, ; type_token_id
		i32 8; java_map_index
	}, 
	; 118
	%struct.TypeMapModuleEntry {
		i32 33555069, ; type_token_id
		i32 17; java_map_index
	}, 
	; 119
	%struct.TypeMapModuleEntry {
		i32 33555071, ; type_token_id
		i32 120; java_map_index
	}, 
	; 120
	%struct.TypeMapModuleEntry {
		i32 33555074, ; type_token_id
		i32 110; java_map_index
	}, 
	; 121
	%struct.TypeMapModuleEntry {
		i32 33555080, ; type_token_id
		i32 214; java_map_index
	}, 
	; 122
	%struct.TypeMapModuleEntry {
		i32 33555082, ; type_token_id
		i32 76; java_map_index
	}, 
	; 123
	%struct.TypeMapModuleEntry {
		i32 33555089, ; type_token_id
		i32 256; java_map_index
	}, 
	; 124
	%struct.TypeMapModuleEntry {
		i32 33555099, ; type_token_id
		i32 188; java_map_index
	}, 
	; 125
	%struct.TypeMapModuleEntry {
		i32 33555104, ; type_token_id
		i32 93; java_map_index
	}, 
	; 126
	%struct.TypeMapModuleEntry {
		i32 33555106, ; type_token_id
		i32 53; java_map_index
	}, 
	; 127
	%struct.TypeMapModuleEntry {
		i32 33555109, ; type_token_id
		i32 88; java_map_index
	}, 
	; 128
	%struct.TypeMapModuleEntry {
		i32 33555111, ; type_token_id
		i32 108; java_map_index
	}, 
	; 129
	%struct.TypeMapModuleEntry {
		i32 33555117, ; type_token_id
		i32 151; java_map_index
	}, 
	; 130
	%struct.TypeMapModuleEntry {
		i32 33555123, ; type_token_id
		i32 253; java_map_index
	}, 
	; 131
	%struct.TypeMapModuleEntry {
		i32 33555137, ; type_token_id
		i32 34; java_map_index
	}, 
	; 132
	%struct.TypeMapModuleEntry {
		i32 33555139, ; type_token_id
		i32 2; java_map_index
	}, 
	; 133
	%struct.TypeMapModuleEntry {
		i32 33555141, ; type_token_id
		i32 186; java_map_index
	}, 
	; 134
	%struct.TypeMapModuleEntry {
		i32 33555143, ; type_token_id
		i32 13; java_map_index
	}, 
	; 135
	%struct.TypeMapModuleEntry {
		i32 33555145, ; type_token_id
		i32 81; java_map_index
	}, 
	; 136
	%struct.TypeMapModuleEntry {
		i32 33555149, ; type_token_id
		i32 322; java_map_index
	}, 
	; 137
	%struct.TypeMapModuleEntry {
		i32 33555151, ; type_token_id
		i32 243; java_map_index
	}, 
	; 138
	%struct.TypeMapModuleEntry {
		i32 33555153, ; type_token_id
		i32 286; java_map_index
	}, 
	; 139
	%struct.TypeMapModuleEntry {
		i32 33555157, ; type_token_id
		i32 48; java_map_index
	}, 
	; 140
	%struct.TypeMapModuleEntry {
		i32 33555159, ; type_token_id
		i32 158; java_map_index
	}, 
	; 141
	%struct.TypeMapModuleEntry {
		i32 33555163, ; type_token_id
		i32 307; java_map_index
	}
], align 4; end of 'module0_managed_to_java_duplicates' array


; module1_managed_to_java
@module1_managed_to_java = internal constant [10 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554476, ; type_token_id
		i32 304; java_map_index
	}, 
	; 1
	%struct.TypeMapModuleEntry {
		i32 33554479, ; type_token_id
		i32 238; java_map_index
	}, 
	; 2
	%struct.TypeMapModuleEntry {
		i32 33554482, ; type_token_id
		i32 28; java_map_index
	}, 
	; 3
	%struct.TypeMapModuleEntry {
		i32 33554484, ; type_token_id
		i32 119; java_map_index
	}, 
	; 4
	%struct.TypeMapModuleEntry {
		i32 33554488, ; type_token_id
		i32 155; java_map_index
	}, 
	; 5
	%struct.TypeMapModuleEntry {
		i32 33554493, ; type_token_id
		i32 235; java_map_index
	}, 
	; 6
	%struct.TypeMapModuleEntry {
		i32 33554523, ; type_token_id
		i32 127; java_map_index
	}, 
	; 7
	%struct.TypeMapModuleEntry {
		i32 33554525, ; type_token_id
		i32 60; java_map_index
	}, 
	; 8
	%struct.TypeMapModuleEntry {
		i32 33554653, ; type_token_id
		i32 11; java_map_index
	}, 
	; 9
	%struct.TypeMapModuleEntry {
		i32 33554654, ; type_token_id
		i32 233; java_map_index
	}
], align 4; end of 'module1_managed_to_java' array


; module2_managed_to_java
@module2_managed_to_java = internal constant [8 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554447, ; type_token_id
		i32 299; java_map_index
	}, 
	; 1
	%struct.TypeMapModuleEntry {
		i32 33554448, ; type_token_id
		i32 70; java_map_index
	}, 
	; 2
	%struct.TypeMapModuleEntry {
		i32 33554449, ; type_token_id
		i32 63; java_map_index
	}, 
	; 3
	%struct.TypeMapModuleEntry {
		i32 33554450, ; type_token_id
		i32 250; java_map_index
	}, 
	; 4
	%struct.TypeMapModuleEntry {
		i32 33554451, ; type_token_id
		i32 115; java_map_index
	}, 
	; 5
	%struct.TypeMapModuleEntry {
		i32 33554452, ; type_token_id
		i32 65; java_map_index
	}, 
	; 6
	%struct.TypeMapModuleEntry {
		i32 33554454, ; type_token_id
		i32 284; java_map_index
	}, 
	; 7
	%struct.TypeMapModuleEntry {
		i32 33554455, ; type_token_id
		i32 50; java_map_index
	}
], align 4; end of 'module2_managed_to_java' array


; module2_managed_to_java_duplicates
@module2_managed_to_java_duplicates = internal constant [3 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554453, ; type_token_id
		i32 65; java_map_index
	}, 
	; 1
	%struct.TypeMapModuleEntry {
		i32 33554456, ; type_token_id
		i32 50; java_map_index
	}, 
	; 2
	%struct.TypeMapModuleEntry {
		i32 33554457, ; type_token_id
		i32 284; java_map_index
	}
], align 4; end of 'module2_managed_to_java_duplicates' array


; module3_managed_to_java
@module3_managed_to_java = internal constant [1 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554435, ; type_token_id
		i32 260; java_map_index
	}
], align 4; end of 'module3_managed_to_java' array


; module3_managed_to_java_duplicates
@module3_managed_to_java_duplicates = internal constant [1 x %struct.TypeMapModuleEntry] [
	; 0
	%struct.TypeMapModuleEntry {
		i32 33554436, ; type_token_id
		i32 260; java_map_index
	}
], align 4; end of 'module3_managed_to_java_duplicates' array

; Map modules
@__TypeMapModule_assembly_name.0 = internal constant [13 x i8] c"Mono.Android\00", align 1
@__TypeMapModule_assembly_name.1 = internal constant [16 x i8] c"TransducerAppXA\00", align 1
@__TypeMapModule_assembly_name.2 = internal constant [22 x i8] c"Xamarin.AndroidX.Core\00", align 1
@__TypeMapModule_assembly_name.3 = internal constant [38 x i8] c"Xamarin.Google.Guava.ListenableFuture\00", align 1

; map_modules
@map_modules = global [4 x %struct.TypeMapModule] [
	; 0
	%struct.TypeMapModule {
		[16 x i8] c"\1B\E1M\9D\E6\00lM\BE\ACcY\95\D0\BAt", ; module_uuid: 9d4de11b-00e6-4d6c-beac-635995d0ba74
		i32 305, ; entry_count
		i32 142, ; duplicate_count
		%struct.TypeMapModuleEntry* getelementptr inbounds ([305 x %struct.TypeMapModuleEntry], [305 x %struct.TypeMapModuleEntry]* @module0_managed_to_java, i32 0, i32 0), ; map
		%struct.TypeMapModuleEntry* getelementptr inbounds ([142 x %struct.TypeMapModuleEntry], [142 x %struct.TypeMapModuleEntry]* @module0_managed_to_java_duplicates, i32 0, i32 0), ; duplicate_map
		i8* getelementptr inbounds ([13 x i8], [13 x i8]* @__TypeMapModule_assembly_name.0, i32 0, i32 0), ; assembly_name: Mono.Android
		%struct.MonoImage* null, ; image
		i32 0, ; java_name_width
		i8* null; java_map
	}, 
	; 1
	%struct.TypeMapModule {
		[16 x i8] c"USVvb\A1fI\A5ZY9\BD\0F\93w", ; module_uuid: 76565355-a162-4966-a55a-5939bd0f9377
		i32 10, ; entry_count
		i32 0, ; duplicate_count
		%struct.TypeMapModuleEntry* getelementptr inbounds ([10 x %struct.TypeMapModuleEntry], [10 x %struct.TypeMapModuleEntry]* @module1_managed_to_java, i32 0, i32 0), ; map
		%struct.TypeMapModuleEntry* null, ; duplicate_map
		i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__TypeMapModule_assembly_name.1, i32 0, i32 0), ; assembly_name: TransducerAppXA
		%struct.MonoImage* null, ; image
		i32 0, ; java_name_width
		i8* null; java_map
	}, 
	; 2
	%struct.TypeMapModule {
		[16 x i8] c"\A5V\AA&o\A2RG\93z\885\CA=\D0y", ; module_uuid: 26aa56a5-a26f-4752-937a-8835ca3dd079
		i32 8, ; entry_count
		i32 3, ; duplicate_count
		%struct.TypeMapModuleEntry* getelementptr inbounds ([8 x %struct.TypeMapModuleEntry], [8 x %struct.TypeMapModuleEntry]* @module2_managed_to_java, i32 0, i32 0), ; map
		%struct.TypeMapModuleEntry* getelementptr inbounds ([3 x %struct.TypeMapModuleEntry], [3 x %struct.TypeMapModuleEntry]* @module2_managed_to_java_duplicates, i32 0, i32 0), ; duplicate_map
		i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__TypeMapModule_assembly_name.2, i32 0, i32 0), ; assembly_name: Xamarin.AndroidX.Core
		%struct.MonoImage* null, ; image
		i32 0, ; java_name_width
		i8* null; java_map
	}, 
	; 3
	%struct.TypeMapModule {
		[16 x i8] c"\D9\85\AB\22\0C\C49G\B6\FE\C7\ACl\FD\02.", ; module_uuid: 22ab85d9-c40c-4739-b6fe-c7ac6cfd022e
		i32 1, ; entry_count
		i32 1, ; duplicate_count
		%struct.TypeMapModuleEntry* getelementptr inbounds ([1 x %struct.TypeMapModuleEntry], [1 x %struct.TypeMapModuleEntry]* @module3_managed_to_java, i32 0, i32 0), ; map
		%struct.TypeMapModuleEntry* getelementptr inbounds ([1 x %struct.TypeMapModuleEntry], [1 x %struct.TypeMapModuleEntry]* @module3_managed_to_java_duplicates, i32 0, i32 0), ; duplicate_map
		i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__TypeMapModule_assembly_name.3, i32 0, i32 0), ; assembly_name: Xamarin.Google.Guava.ListenableFuture
		%struct.MonoImage* null, ; image
		i32 0, ; java_name_width
		i8* null; java_map
	}
], align 4; end of 'map_modules' array


; Java to managed map

; map_java
@map_java = local_unnamed_addr constant [324 x %struct.TypeMapJava] [
	; 0
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554832, ; type_token_id
		i32 123; java_name_index
	}, 
	; 1
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555124, ; type_token_id
		i32 276; java_name_index
	}, 
	; 2
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 288; java_name_index
	}, 
	; 3
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 40; java_name_index
	}, 
	; 4
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555041, ; type_token_id
		i32 221; java_name_index
	}, 
	; 5
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 42; java_name_index
	}, 
	; 6
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554756, ; type_token_id
		i32 90; java_name_index
	}, 
	; 7
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554792, ; type_token_id
		i32 103; java_name_index
	}, 
	; 8
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 234; java_name_index
	}, 
	; 9
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 9; java_name_index
	}, 
	; 10
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554729, ; type_token_id
		i32 76; java_name_index
	}, 
	; 11
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554653, ; type_token_id
		i32 312; java_name_index
	}, 
	; 12
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 108; java_name_index
	}, 
	; 13
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 290; java_name_index
	}, 
	; 14
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555112, ; type_token_id
		i32 266; java_name_index
	}, 
	; 15
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 16; java_name_index
	}, 
	; 16
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 229; java_name_index
	}, 
	; 17
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 235; java_name_index
	}, 
	; 18
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555125, ; type_token_id
		i32 277; java_name_index
	}, 
	; 19
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554828, ; type_token_id
		i32 119; java_name_index
	}, 
	; 20
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 102; java_name_index
	}, 
	; 21
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554579, ; type_token_id
		i32 2; java_name_index
	}, 
	; 22
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 190; java_name_index
	}, 
	; 23
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 66; java_name_index
	}, 
	; 24
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554823, ; type_token_id
		i32 116; java_name_index
	}, 
	; 25
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555086, ; type_token_id
		i32 248; java_name_index
	}, 
	; 26
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554757, ; type_token_id
		i32 91; java_name_index
	}, 
	; 27
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555132, ; type_token_id
		i32 283; java_name_index
	}, 
	; 28
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554482, ; type_token_id
		i32 309; java_name_index
	}, 
	; 29
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 218; java_name_index
	}, 
	; 30
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555119, ; type_token_id
		i32 272; java_name_index
	}, 
	; 31
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 210; java_name_index
	}, 
	; 32
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555072, ; type_token_id
		i32 237; java_name_index
	}, 
	; 33
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555118, ; type_token_id
		i32 271; java_name_index
	}, 
	; 34
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 287; java_name_index
	}, 
	; 35
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554619, ; type_token_id
		i32 19; java_name_index
	}, 
	; 36
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555032, ; type_token_id
		i32 215; java_name_index
	}, 
	; 37
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554758, ; type_token_id
		i32 92; java_name_index
	}, 
	; 38
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 79; java_name_index
	}, 
	; 39
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 72; java_name_index
	}, 
	; 40
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554843, ; type_token_id
		i32 133; java_name_index
	}, 
	; 41
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555009, ; type_token_id
		i32 207; java_name_index
	}, 
	; 42
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 185; java_name_index
	}, 
	; 43
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555052, ; type_token_id
		i32 227; java_name_index
	}, 
	; 44
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554651, ; type_token_id
		i32 32; java_name_index
	}, 
	; 45
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555113, ; type_token_id
		i32 267; java_name_index
	}, 
	; 46
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554760, ; type_token_id
		i32 93; java_name_index
	}, 
	; 47
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554837, ; type_token_id
		i32 128; java_name_index
	}, 
	; 48
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 299; java_name_index
	}, 
	; 49
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555087, ; type_token_id
		i32 249; java_name_index
	}, 
	; 50
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 0, ; type_token_id
		i32 322; java_name_index
	}, 
	; 51
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554630, ; type_token_id
		i32 24; java_name_index
	}, 
	; 52
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554988, ; type_token_id
		i32 203; java_name_index
	}, 
	; 53
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 263; java_name_index
	}, 
	; 54
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555160, ; type_token_id
		i32 301; java_name_index
	}, 
	; 55
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555115, ; type_token_id
		i32 269; java_name_index
	}, 
	; 56
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555131, ; type_token_id
		i32 282; java_name_index
	}, 
	; 57
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555121, ; type_token_id
		i32 274; java_name_index
	}, 
	; 58
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554675, ; type_token_id
		i32 48; java_name_index
	}, 
	; 59
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 10; java_name_index
	}, 
	; 60
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554525, ; type_token_id
		i32 307; java_name_index
	}, 
	; 61
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554910, ; type_token_id
		i32 176; java_name_index
	}, 
	; 62
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555128, ; type_token_id
		i32 280; java_name_index
	}, 
	; 63
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554449, ; type_token_id
		i32 317; java_name_index
	}, 
	; 64
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 166; java_name_index
	}, 
	; 65
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 0, ; type_token_id
		i32 320; java_name_index
	}, 
	; 66
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555050, ; type_token_id
		i32 226; java_name_index
	}, 
	; 67
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 87; java_name_index
	}, 
	; 68
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554657, ; type_token_id
		i32 36; java_name_index
	}, 
	; 69
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554870, ; type_token_id
		i32 155; java_name_index
	}, 
	; 70
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554448, ; type_token_id
		i32 316; java_name_index
	}, 
	; 71
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 77; java_name_index
	}, 
	; 72
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 230; java_name_index
	}, 
	; 73
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554863, ; type_token_id
		i32 148; java_name_index
	}, 
	; 74
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554650, ; type_token_id
		i32 31; java_name_index
	}, 
	; 75
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554771, ; type_token_id
		i32 99; java_name_index
	}, 
	; 76
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555081, ; type_token_id
		i32 244; java_name_index
	}, 
	; 77
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554887, ; type_token_id
		i32 165; java_name_index
	}, 
	; 78
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554943, ; type_token_id
		i32 195; java_name_index
	}, 
	; 79
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 219; java_name_index
	}, 
	; 80
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554885, ; type_token_id
		i32 164; java_name_index
	}, 
	; 81
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 291; java_name_index
	}, 
	; 82
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 212; java_name_index
	}, 
	; 83
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554598, ; type_token_id
		i32 12; java_name_index
	}, 
	; 84
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554831, ; type_token_id
		i32 122; java_name_index
	}, 
	; 85
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 96; java_name_index
	}, 
	; 86
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 209; java_name_index
	}, 
	; 87
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554674, ; type_token_id
		i32 47; java_name_index
	}, 
	; 88
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 264; java_name_index
	}, 
	; 89
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554858, ; type_token_id
		i32 143; java_name_index
	}, 
	; 90
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555096, ; type_token_id
		i32 256; java_name_index
	}, 
	; 91
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 70; java_name_index
	}, 
	; 92
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554848, ; type_token_id
		i32 136; java_name_index
	}, 
	; 93
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 262; java_name_index
	}, 
	; 94
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554577, ; type_token_id
		i32 1; java_name_index
	}, 
	; 95
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 98; java_name_index
	}, 
	; 96
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554860, ; type_token_id
		i32 145; java_name_index
	}, 
	; 97
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555084, ; type_token_id
		i32 246; java_name_index
	}, 
	; 98
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554850, ; type_token_id
		i32 138; java_name_index
	}, 
	; 99
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555133, ; type_token_id
		i32 284; java_name_index
	}, 
	; 100
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 39; java_name_index
	}, 
	; 101
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554599, ; type_token_id
		i32 13; java_name_index
	}, 
	; 102
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554977, ; type_token_id
		i32 201; java_name_index
	}, 
	; 103
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554865, ; type_token_id
		i32 150; java_name_index
	}, 
	; 104
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555095, ; type_token_id
		i32 255; java_name_index
	}, 
	; 105
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554872, ; type_token_id
		i32 157; java_name_index
	}, 
	; 106
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 7; java_name_index
	}, 
	; 107
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554907, ; type_token_id
		i32 174; java_name_index
	}, 
	; 108
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 265; java_name_index
	}, 
	; 109
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555127, ; type_token_id
		i32 279; java_name_index
	}, 
	; 110
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555073, ; type_token_id
		i32 238; java_name_index
	}, 
	; 111
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554874, ; type_token_id
		i32 159; java_name_index
	}, 
	; 112
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 71; java_name_index
	}, 
	; 113
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554671, ; type_token_id
		i32 44; java_name_index
	}, 
	; 114
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554673, ; type_token_id
		i32 46; java_name_index
	}, 
	; 115
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554451, ; type_token_id
		i32 319; java_name_index
	}, 
	; 116
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554979, ; type_token_id
		i32 202; java_name_index
	}, 
	; 117
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554581, ; type_token_id
		i32 3; java_name_index
	}, 
	; 118
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554851, ; type_token_id
		i32 139; java_name_index
	}, 
	; 119
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554484, ; type_token_id
		i32 310; java_name_index
	}, 
	; 120
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555070, ; type_token_id
		i32 236; java_name_index
	}, 
	; 121
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 213; java_name_index
	}, 
	; 122
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554640, ; type_token_id
		i32 27; java_name_index
	}, 
	; 123
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554857, ; type_token_id
		i32 142; java_name_index
	}, 
	; 124
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555146, ; type_token_id
		i32 292; java_name_index
	}, 
	; 125
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554725, ; type_token_id
		i32 73; java_name_index
	}, 
	; 126
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554908, ; type_token_id
		i32 175; java_name_index
	}, 
	; 127
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554523, ; type_token_id
		i32 306; java_name_index
	}, 
	; 128
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554894, ; type_token_id
		i32 168; java_name_index
	}, 
	; 129
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554703, ; type_token_id
		i32 61; java_name_index
	}, 
	; 130
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554839, ; type_token_id
		i32 130; java_name_index
	}, 
	; 131
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 228; java_name_index
	}, 
	; 132
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 233; java_name_index
	}, 
	; 133
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 216; java_name_index
	}, 
	; 134
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554864, ; type_token_id
		i32 149; java_name_index
	}, 
	; 135
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554740, ; type_token_id
		i32 83; java_name_index
	}, 
	; 136
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555126, ; type_token_id
		i32 278; java_name_index
	}, 
	; 137
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555114, ; type_token_id
		i32 268; java_name_index
	}, 
	; 138
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555092, ; type_token_id
		i32 252; java_name_index
	}, 
	; 139
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554641, ; type_token_id
		i32 28; java_name_index
	}, 
	; 140
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555093, ; type_token_id
		i32 253; java_name_index
	}, 
	; 141
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554655, ; type_token_id
		i32 34; java_name_index
	}, 
	; 142
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555025, ; type_token_id
		i32 211; java_name_index
	}, 
	; 143
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554834, ; type_token_id
		i32 125; java_name_index
	}, 
	; 144
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554906, ; type_token_id
		i32 173; java_name_index
	}, 
	; 145
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554869, ; type_token_id
		i32 154; java_name_index
	}, 
	; 146
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554941, ; type_token_id
		i32 193; java_name_index
	}, 
	; 147
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 113; java_name_index
	}, 
	; 148
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 85; java_name_index
	}, 
	; 149
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554847, ; type_token_id
		i32 135; java_name_index
	}, 
	; 150
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555043, ; type_token_id
		i32 222; java_name_index
	}, 
	; 151
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 270; java_name_index
	}, 
	; 152
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554670, ; type_token_id
		i32 43; java_name_index
	}, 
	; 153
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554739, ; type_token_id
		i32 82; java_name_index
	}, 
	; 154
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 231; java_name_index
	}, 
	; 155
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554488, ; type_token_id
		i32 311; java_name_index
	}, 
	; 156
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554825, ; type_token_id
		i32 117; java_name_index
	}, 
	; 157
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 23; java_name_index
	}, 
	; 158
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555158, ; type_token_id
		i32 300; java_name_index
	}, 
	; 159
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 111; java_name_index
	}, 
	; 160
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554927, ; type_token_id
		i32 186; java_name_index
	}, 
	; 161
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554893, ; type_token_id
		i32 167; java_name_index
	}, 
	; 162
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555091, ; type_token_id
		i32 251; java_name_index
	}, 
	; 163
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 21; java_name_index
	}, 
	; 164
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554930, ; type_token_id
		i32 188; java_name_index
	}, 
	; 165
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554786, ; type_token_id
		i32 101; java_name_index
	}, 
	; 166
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554656, ; type_token_id
		i32 35; java_name_index
	}, 
	; 167
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554702, ; type_token_id
		i32 60; java_name_index
	}, 
	; 168
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554621, ; type_token_id
		i32 20; java_name_index
	}, 
	; 169
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 187; java_name_index
	}, 
	; 170
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 223; java_name_index
	}, 
	; 171
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554884, ; type_token_id
		i32 163; java_name_index
	}, 
	; 172
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 41; java_name_index
	}, 
	; 173
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554895, ; type_token_id
		i32 169; java_name_index
	}, 
	; 174
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554931, ; type_token_id
		i32 189; java_name_index
	}, 
	; 175
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555040, ; type_token_id
		i32 220; java_name_index
	}, 
	; 176
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554838, ; type_token_id
		i32 129; java_name_index
	}, 
	; 177
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554868, ; type_token_id
		i32 153; java_name_index
	}, 
	; 178
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554905, ; type_token_id
		i32 172; java_name_index
	}, 
	; 179
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554942, ; type_token_id
		i32 194; java_name_index
	}, 
	; 180
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554913, ; type_token_id
		i32 178; java_name_index
	}, 
	; 181
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554645, ; type_token_id
		i32 30; java_name_index
	}, 
	; 182
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554866, ; type_token_id
		i32 151; java_name_index
	}, 
	; 183
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555085, ; type_token_id
		i32 247; java_name_index
	}, 
	; 184
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555078, ; type_token_id
		i32 242; java_name_index
	}, 
	; 185
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554896, ; type_token_id
		i32 170; java_name_index
	}, 
	; 186
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 289; java_name_index
	}, 
	; 187
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554846, ; type_token_id
		i32 134; java_name_index
	}, 
	; 188
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555098, ; type_token_id
		i32 258; java_name_index
	}, 
	; 189
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554685, ; type_token_id
		i32 53; java_name_index
	}, 
	; 190
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 88; java_name_index
	}, 
	; 191
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555134, ; type_token_id
		i32 285; java_name_index
	}, 
	; 192
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554944, ; type_token_id
		i32 196; java_name_index
	}, 
	; 193
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555083, ; type_token_id
		i32 245; java_name_index
	}, 
	; 194
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 181; java_name_index
	}, 
	; 195
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 17; java_name_index
	}, 
	; 196
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 232; java_name_index
	}, 
	; 197
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 191; java_name_index
	}, 
	; 198
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 63; java_name_index
	}, 
	; 199
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555147, ; type_token_id
		i32 293; java_name_index
	}, 
	; 200
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555046, ; type_token_id
		i32 224; java_name_index
	}, 
	; 201
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554638, ; type_token_id
		i32 26; java_name_index
	}, 
	; 202
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 33; java_name_index
	}, 
	; 203
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554946, ; type_token_id
		i32 198; java_name_index
	}, 
	; 204
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555154, ; type_token_id
		i32 297; java_name_index
	}, 
	; 205
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555135, ; type_token_id
		i32 286; java_name_index
	}, 
	; 206
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 6; java_name_index
	}, 
	; 207
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 37; java_name_index
	}, 
	; 208
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554738, ; type_token_id
		i32 81; java_name_index
	}, 
	; 209
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554686, ; type_token_id
		i32 54; java_name_index
	}, 
	; 210
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554795, ; type_token_id
		i32 105; java_name_index
	}, 
	; 211
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 38; java_name_index
	}, 
	; 212
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554880, ; type_token_id
		i32 161; java_name_index
	}, 
	; 213
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554741, ; type_token_id
		i32 84; java_name_index
	}, 
	; 214
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555079, ; type_token_id
		i32 243; java_name_index
	}, 
	; 215
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 118; java_name_index
	}, 
	; 216
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 112; java_name_index
	}, 
	; 217
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555102, ; type_token_id
		i32 261; java_name_index
	}, 
	; 218
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554835, ; type_token_id
		i32 126; java_name_index
	}, 
	; 219
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554683, ; type_token_id
		i32 51; java_name_index
	}, 
	; 220
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554704, ; type_token_id
		i32 62; java_name_index
	}, 
	; 221
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554859, ; type_token_id
		i32 144; java_name_index
	}, 
	; 222
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554696, ; type_token_id
		i32 57; java_name_index
	}, 
	; 223
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554945, ; type_token_id
		i32 197; java_name_index
	}, 
	; 224
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 183; java_name_index
	}, 
	; 225
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554861, ; type_token_id
		i32 146; java_name_index
	}, 
	; 226
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554717, ; type_token_id
		i32 69; java_name_index
	}, 
	; 227
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 104; java_name_index
	}, 
	; 228
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555101, ; type_token_id
		i32 260; java_name_index
	}, 
	; 229
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554672, ; type_token_id
		i32 45; java_name_index
	}, 
	; 230
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 200; java_name_index
	}, 
	; 231
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555187, ; type_token_id
		i32 304; java_name_index
	}, 
	; 232
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554684, ; type_token_id
		i32 52; java_name_index
	}, 
	; 233
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554654, ; type_token_id
		i32 313; java_name_index
	}, 
	; 234
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555161, ; type_token_id
		i32 302; java_name_index
	}, 
	; 235
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554493, ; type_token_id
		i32 314; java_name_index
	}, 
	; 236
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 214; java_name_index
	}, 
	; 237
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554833, ; type_token_id
		i32 124; java_name_index
	}, 
	; 238
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554479, ; type_token_id
		i32 308; java_name_index
	}, 
	; 239
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 58; java_name_index
	}, 
	; 240
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554728, ; type_token_id
		i32 75; java_name_index
	}, 
	; 241
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 74; java_name_index
	}, 
	; 242
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 49; java_name_index
	}, 
	; 243
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 295; java_name_index
	}, 
	; 244
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554750, ; type_token_id
		i32 89; java_name_index
	}, 
	; 245
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 68; java_name_index
	}, 
	; 246
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 8; java_name_index
	}, 
	; 247
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555077, ; type_token_id
		i32 241; java_name_index
	}, 
	; 248
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554679, ; type_token_id
		i32 50; java_name_index
	}, 
	; 249
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 107; java_name_index
	}, 
	; 250
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554450, ; type_token_id
		i32 318; java_name_index
	}, 
	; 251
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554637, ; type_token_id
		i32 25; java_name_index
	}, 
	; 252
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554584, ; type_token_id
		i32 5; java_name_index
	}, 
	; 253
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555122, ; type_token_id
		i32 275; java_name_index
	}, 
	; 254
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554576, ; type_token_id
		i32 0; java_name_index
	}, 
	; 255
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 192; java_name_index
	}, 
	; 256
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555088, ; type_token_id
		i32 250; java_name_index
	}, 
	; 257
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554830, ; type_token_id
		i32 121; java_name_index
	}, 
	; 258
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554992, ; type_token_id
		i32 205; java_name_index
	}, 
	; 259
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554829, ; type_token_id
		i32 120; java_name_index
	}, 
	; 260
	%struct.TypeMapJava {
		i32 3, ; module_index
		i32 0, ; type_token_id
		i32 323; java_name_index
	}, 
	; 261
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554616, ; type_token_id
		i32 18; java_name_index
	}, 
	; 262
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554732, ; type_token_id
		i32 78; java_name_index
	}, 
	; 263
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555129, ; type_token_id
		i32 281; java_name_index
	}, 
	; 264
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555075, ; type_token_id
		i32 239; java_name_index
	}, 
	; 265
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554785, ; type_token_id
		i32 100; java_name_index
	}, 
	; 266
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554990, ; type_token_id
		i32 204; java_name_index
	}, 
	; 267
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554700, ; type_token_id
		i32 59; java_name_index
	}, 
	; 268
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554819, ; type_token_id
		i32 115; java_name_index
	}, 
	; 269
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554867, ; type_token_id
		i32 152; java_name_index
	}, 
	; 270
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555097, ; type_token_id
		i32 257; java_name_index
	}, 
	; 271
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 95; java_name_index
	}, 
	; 272
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554836, ; type_token_id
		i32 127; java_name_index
	}, 
	; 273
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 110; java_name_index
	}, 
	; 274
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554582, ; type_token_id
		i32 4; java_name_index
	}, 
	; 275
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555076, ; type_token_id
		i32 240; java_name_index
	}, 
	; 276
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 162; java_name_index
	}, 
	; 277
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554767, ; type_token_id
		i32 97; java_name_index
	}, 
	; 278
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555020, ; type_token_id
		i32 208; java_name_index
	}, 
	; 279
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554854, ; type_token_id
		i32 141; java_name_index
	}, 
	; 280
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554687, ; type_token_id
		i32 55; java_name_index
	}, 
	; 281
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 11; java_name_index
	}, 
	; 282
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555094, ; type_token_id
		i32 254; java_name_index
	}, 
	; 283
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 80; java_name_index
	}, 
	; 284
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554454, ; type_token_id
		i32 321; java_name_index
	}, 
	; 285
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554875, ; type_token_id
		i32 160; java_name_index
	}, 
	; 286
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555152, ; type_token_id
		i32 296; java_name_index
	}, 
	; 287
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 109; java_name_index
	}, 
	; 288
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555035, ; type_token_id
		i32 217; java_name_index
	}, 
	; 289
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 64; java_name_index
	}, 
	; 290
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554849, ; type_token_id
		i32 137; java_name_index
	}, 
	; 291
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554924, ; type_token_id
		i32 184; java_name_index
	}, 
	; 292
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554873, ; type_token_id
		i32 158; java_name_index
	}, 
	; 293
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554688, ; type_token_id
		i32 56; java_name_index
	}, 
	; 294
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554972, ; type_token_id
		i32 199; java_name_index
	}, 
	; 295
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554600, ; type_token_id
		i32 14; java_name_index
	}, 
	; 296
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554916, ; type_token_id
		i32 180; java_name_index
	}, 
	; 297
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554862, ; type_token_id
		i32 147; java_name_index
	}, 
	; 298
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555100, ; type_token_id
		i32 259; java_name_index
	}, 
	; 299
	%struct.TypeMapJava {
		i32 2, ; module_index
		i32 33554447, ; type_token_id
		i32 315; java_name_index
	}, 
	; 300
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554897, ; type_token_id
		i32 171; java_name_index
	}, 
	; 301
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 182; java_name_index
	}, 
	; 302
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554914, ; type_token_id
		i32 179; java_name_index
	}, 
	; 303
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555120, ; type_token_id
		i32 273; java_name_index
	}, 
	; 304
	%struct.TypeMapJava {
		i32 1, ; module_index
		i32 33554476, ; type_token_id
		i32 305; java_name_index
	}, 
	; 305
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555048, ; type_token_id
		i32 225; java_name_index
	}, 
	; 306
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554744, ; type_token_id
		i32 86; java_name_index
	}, 
	; 307
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555162, ; type_token_id
		i32 303; java_name_index
	}, 
	; 308
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554842, ; type_token_id
		i32 132; java_name_index
	}, 
	; 309
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554625, ; type_token_id
		i32 22; java_name_index
	}, 
	; 310
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554797, ; type_token_id
		i32 106; java_name_index
	}, 
	; 311
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554602, ; type_token_id
		i32 15; java_name_index
	}, 
	; 312
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33555155, ; type_token_id
		i32 298; java_name_index
	}, 
	; 313
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 114; java_name_index
	}, 
	; 314
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554912, ; type_token_id
		i32 177; java_name_index
	}, 
	; 315
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554993, ; type_token_id
		i32 206; java_name_index
	}, 
	; 316
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 94; java_name_index
	}, 
	; 317
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 131; java_name_index
	}, 
	; 318
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 67; java_name_index
	}, 
	; 319
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 65; java_name_index
	}, 
	; 320
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554852, ; type_token_id
		i32 140; java_name_index
	}, 
	; 321
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 33554871, ; type_token_id
		i32 156; java_name_index
	}, 
	; 322
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 294; java_name_index
	}, 
	; 323
	%struct.TypeMapJava {
		i32 0, ; module_index
		i32 0, ; type_token_id
		i32 29; java_name_index
	}
], align 4; end of 'map_java' array

@map_java_hashes = local_unnamed_addr constant [324 x i32] [
	i32 9160146, ; 0: 0x8bc5d2 => android/provider/Settings$Secure
	i32 12341354, ; 1: 0xbc506a => java/lang/Object
	i32 13389226, ; 2: 0xcc4daa => java/lang/reflect/GenericDeclaration
	i32 29653966, ; 3: 0x1c47bce => android/widget/ListAdapter
	i32 32078366, ; 4: 0x1e97a1e => java/security/cert/Certificate
	i32 67977947, ; 5: 0x40d42db => android/widget/ThemedSpinnerAdapter
	i32 74282880, ; 6: 0x46d7780 => android/view/ViewGroup
	i32 118977103, ; 7: 0x717724f => android/util/DisplayMetrics
	i32 133154022, ; 8: 0x7efc4e6 => java/nio/channels/SeekableByteChannel
	i32 138171443, ; 9: 0x83c5433 => javax/net/ssl/SSLSessionContext
	i32 139280357, ; 10: 0x84d3fe5 => android/view/KeyEvent
	i32 142108376, ; 11: 0x87866d8 => crc6475b0d5b881bfcfb5/WifiConnector_SpecifierNetworkCallback
	i32 148505617, ; 12: 0x8da0411 => android/text/GetChars
	i32 151062962, ; 13: 0x90109b2 => java/lang/reflect/TypeVariable
	i32 176697843, ; 14: 0xa8831f3 => java/lang/IllegalArgumentException
	i32 178346187, ; 15: 0xaa158cb => android/window/OnBackInvokedCallback
	i32 182338948, ; 16: 0xade4584 => java/nio/channels/Channel
	i32 229694295, ; 17: 0xdb0db57 => java/nio/channels/WritableByteChannel
	i32 257094054, ; 18: 0xf52f1a6 => java/lang/ReflectiveOperationException
	i32 262602588, ; 19: 0xfa6ff5c => android/provider/MediaStore
	i32 268673672, ; 20: 0x1003a288 => android/view/accessibility/AccessibilityEventSource
	i32 269199815, ; 21: 0x100ba9c7 => javax/security/cert/X509Certificate
	i32 279693177, ; 22: 0x10abc779 => android/content/SharedPreferences$Editor
	i32 307048059, ; 23: 0x124d2e7b => android/view/MenuItem$OnActionExpandListener
	i32 358279401, ; 24: 0x155ae8e9 => android/text/style/CharacterStyle
	i32 362231028, ; 25: 0x159734f4 => java/net/URI
	i32 366534601, ; 26: 0x15d8dfc9 => android/view/ViewGroup$LayoutParams
	i32 393371378, ; 27: 0x17725ef2 => mono/java/lang/RunnableImplementor
	i32 397042180, ; 28: 0x17aa6204 => crc64d4bf71c1e765891b/TestAndroidLayout
	i32 412395228, ; 29: 0x1894a6dc => java/security/KeyStore$LoadStoreParameter
	i32 412771173, ; 30: 0x189a6365 => java/lang/Long
	i32 419359493, ; 31: 0x18feeb05 => java/util/Iterator
	i32 420482824, ; 32: 0x19100f08 => java/net/ConnectException
	i32 443233435, ; 33: 0x1a6b349b => java/lang/LinkageError
	i32 466997013, ; 34: 0x1bd5cf15 => java/lang/reflect/AnnotatedElement
	i32 467220734, ; 35: 0x1bd938fe => android/widget/AbsSpinner
	i32 490619983, ; 36: 0x1d3e444f => java/util/concurrent/TimeUnit
	i32 501733478, ; 37: 0x1de7d866 => android/view/ViewGroup$MarginLayoutParams
	i32 509491678, ; 38: 0x1e5e39de => android/view/LayoutInflater$Factory
	i32 517025718, ; 39: 0x1ed12fb6 => android/view/ViewParent
	i32 517668398, ; 40: 0x1edafe2e => android/os/Parcel
	i32 531198748, ; 41: 0x1fa9731c => mono/android/runtime/OutputStreamAdapter
	i32 568462196, ; 42: 0x21e20b74 => android/content/DialogInterface$OnDismissListener
	i32 581097368, ; 43: 0x22a2d798 => java/nio/channels/FileChannel
	i32 584201455, ; 44: 0x22d234ef => android/widget/Filter
	i32 584231583, ; 45: 0x22d2aa9f => java/lang/IllegalStateException
	i32 590702782, ; 46: 0x233568be => android/view/ViewTreeObserver
	i32 591810476, ; 47: 0x23464fac => android/os/Bundle
	i32 606085292, ; 48: 0x242020ac => java/io/Serializable
	i32 619060219, ; 49: 0x24e61bfb => java/net/URL
	i32 657696663, ; 50: 0x2733a797 => androidx/core/app/SharedElementCallback$OnSharedElementsReadyListener
	i32 673806054, ; 51: 0x282976e6 => mono/android/widget/AdapterView_OnItemSelectedListenerImplementor
	i32 692920175, ; 52: 0x294d1f6f => java/util/ArrayList
	i32 780408360, ; 53: 0x2e841628 => java/lang/CharSequence
	i32 780987551, ; 54: 0x2e8cec9f => java/io/PrintWriter
	i32 793918146, ; 55: 0x2f523ac2 => java/lang/Integer
	i32 806800039, ; 56: 0x3016caa7 => java/lang/Thread
	i32 838682992, ; 57: 0x31fd4970 => java/lang/NullPointerException
	i32 868765592, ; 58: 0x33c84f98 => android/widget/RadioGroup
	i32 876646173, ; 59: 0x34408f1d => javax/net/ssl/TrustManager
	i32 880592589, ; 60: 0x347cc6cd => crc64d4bf71c1e765891b/MainActivity_TraceView
	i32 885223365, ; 61: 0x34c36fc5 => android/content/ContentResolver
	i32 893363610, ; 62: 0x353fa59a => java/lang/Short
	i32 899478241, ; 63: 0x359cf2e1 => androidx/core/content/FileProvider
	i32 906034180, ; 64: 0x3600fc04 => android/database/Cursor
	i32 924972967, ; 65: 0x3721f7a7 => androidx/core/app/ActivityCompat$PermissionCompatDelegate
	i32 925357775, ; 66: 0x3727d6cf => java/nio/ByteBuffer
	i32 968142514, ; 67: 0x39b4aeb2 => android/view/View$OnCreateContextMenuListener
	i32 982326989, ; 68: 0x3a8d1ecd => android/widget/HorizontalScrollView
	i32 984605620, ; 69: 0x3aafe3b4 => android/graphics/PointF
	i32 986059584, ; 70: 0x3ac61340 => androidx/core/content/ContextCompat
	i32 988336100, ; 71: 0x3ae8cfe4 => android/view/KeyEvent$Callback
	i32 998009430, ; 72: 0x3b7c6a56 => java/nio/channels/GatheringByteChannel
	i32 1008962460, ; 73: 0x3c238b9c => android/graphics/Color
	i32 1018791985, ; 74: 0x3cb98831 => android/widget/EditText
	i32 1026417919, ; 75: 0x3d2de4ff => android/view/WindowMetrics
	i32 1026507328, ; 76: 0x3d2f4240 => java/net/SocketAddress
	i32 1030707578, ; 77: 0x3d6f597a => android/database/DataSetObserver
	i32 1035992969, ; 78: 0x3dbfff89 => android/content/res/Resources
	i32 1062235695, ; 79: 0x3f506e2f => java/security/KeyStore$ProtectionParameter
	i32 1070459310, ; 80: 0x3fcde9ae => android/database/ContentObserver
	i32 1073016658, ; 81: 0x3ff4ef52 => java/lang/annotation/Annotation
	i32 1077629184, ; 82: 0x403b5100 => java/util/function/Consumer
	i32 1090939588, ; 83: 0x41066ac4 => javax/net/ssl/KeyManagerFactory
	i32 1102556300, ; 84: 0x41b7ac8c => android/provider/Settings$NameValueTable
	i32 1117937440, ; 85: 0x42a25f20 => android/view/ViewTreeObserver$OnTouchModeChangeListener
	i32 1142011573, ; 86: 0x4411b6b5 => java/util/Enumeration
	i32 1146395494, ; 87: 0x44549b66 => android/widget/RadioButton
	i32 1149267780, ; 88: 0x44806f44 => java/lang/Cloneable
	i32 1172217576, ; 89: 0x45de9ee8 => android/net/wifi/WifiManager
	i32 1175636112, ; 90: 0x4612c890 => java/lang/ClassNotFoundException
	i32 1185273701, ; 91: 0x46a5d765 => android/view/SubMenu
	i32 1195821077, ; 92: 0x4746c815 => android/net/Network
	i32 1196063310, ; 93: 0x474a7a4e => java/lang/Appendable
	i32 1227075600, ; 94: 0x4923b010 => javax/security/cert/Certificate
	i32 1270186925, ; 95: 0x4bb583ad => android/view/Window$Callback
	i32 1270375574, ; 96: 0x4bb86496 => android/net/wifi/WifiNetworkSpecifier$Builder
	i32 1270561450, ; 97: 0x4bbb3aaa => java/net/SocketTimeoutException
	i32 1287751596, ; 98: 0x4cc187ac => android/net/NetworkRequest
	i32 1298454265, ; 99: 0x4d64d6f9 => java/lang/Throwable
	i32 1318092535, ; 100: 0x4e907ef7 => android/widget/Filterable
	i32 1323697755, ; 101: 0x4ee6065b => javax/net/ssl/SSLContext
	i32 1335098580, ; 102: 0x4f93fcd4 => java/util/Collection
	i32 1340347874, ; 103: 0x4fe415e2 => android/graphics/Paint
	i32 1368421702, ; 104: 0x51907546 => java/lang/ClassCastException
	i32 1370891736, ; 105: 0x51b625d8 => android/graphics/PorterDuff$Mode
	i32 1373631042, ; 106: 0x51dff242 => javax/net/ssl/KeyManager
	i32 1386757446, ; 107: 0x52a83d46 => android/content/ComponentName
	i32 1388906712, ; 108: 0x52c908d8 => java/lang/Comparable
	i32 1425790689, ; 109: 0x54fbd6e1 => java/lang/SecurityException
	i32 1428048664, ; 110: 0x551e4b18 => java/net/HttpURLConnection
	i32 1429796945, ; 111: 0x5538f851 => android/graphics/RectF
	i32 1433059198, ; 112: 0x556abf7e => android/view/ViewManager
	i32 1447309214, ; 113: 0x56442f9e => android/widget/LinearLayout$LayoutParams
	i32 1459844378, ; 114: 0x5703751a => android/widget/ProgressBar
	i32 1472468295, ; 115: 0x57c41547 => androidx/core/app/ActivityCompat
	i32 1475682991, ; 116: 0x57f522af => java/util/HashMap
	i32 1476293262, ; 117: 0x57fe728e => javax/security/auth/Subject
	i32 1477518586, ; 118: 0x581124fa => android/net/NetworkRequest$Builder
	i32 1485422276, ; 119: 0x5889bec4 => crc64d4bf71c1e765891b/TestRunnerActivity
	i32 1489594546, ; 120: 0x58c968b2 => java/nio/channels/spi/AbstractInterruptibleChannel
	i32 1492815417, ; 121: 0x58fa8e39 => java/util/concurrent/Executor
	i32 1506774891, ; 122: 0x59cf8f6b => android/widget/Button
	i32 1540678432, ; 123: 0x5bd4e320 => android/net/wifi/WifiConfiguration
	i32 1544613420, ; 124: 0x5c10ee2c => java/io/File
	i32 1548306256, ; 125: 0x5c494750 => android/view/WindowManager$LayoutParams
	i32 1550531333, ; 126: 0x5c6b3b05 => android/content/ContentProvider
	i32 1550811971, ; 127: 0x5c6f8343 => crc64d4bf71c1e765891b/MainActivity_CustomStringAdapter
	i32 1573833883, ; 128: 0x5dcecc9b => android/app/AlertDialog
	i32 1584672329, ; 129: 0x5e742e49 => android/view/Display
	i32 1586851388, ; 130: 0x5e956e3c => android/os/Handler
	i32 1595725058, ; 131: 0x5f1cd502 => java/nio/channels/ByteChannel
	i32 1605789814, ; 132: 0x5fb66876 => java/nio/channels/ScatteringByteChannel
	i32 1637959351, ; 133: 0x61a146b7 => java/security/Principal
	i32 1644876130, ; 134: 0x620ad162 => android/graphics/Matrix
	i32 1646348278, ; 135: 0x622147f6 => android/view/View
	i32 1649695927, ; 136: 0x62545cb7 => java/lang/RuntimeException
	i32 1657134862, ; 137: 0x62c5df0e => java/lang/IndexOutOfBoundsException
	i32 1680835779, ; 138: 0x642f84c3 => java/lang/Byte
	i32 1699467861, ; 139: 0x654bd255 => android/widget/CompoundButton
	i32 1718265030, ; 140: 0x666aa4c6 => java/lang/Character
	i32 1740814247, ; 141: 0x67c2b7a7 => android/widget/FrameLayout
	i32 1755285137, ; 142: 0x689f8691 => java/util/Random
	i32 1758490869, ; 143: 0x68d070f5 => android/os/BaseBundle
	i32 1761245836, ; 144: 0x68fa7a8c => android/content/ClipData
	i32 1772705556, ; 145: 0x69a95714 => android/graphics/Point
	i32 1775355160, ; 146: 0x69d1c518 => android/content/res/ColorStateList
	i32 1790236887, ; 147: 0x6ab4d8d7 => android/text/Spanned
	i32 1807220671, ; 148: 0x6bb7ffbf => android/view/View$OnClickListener
	i32 1817676547, ; 149: 0x6c578b03 => android/net/ConnectivityManager$NetworkCallback
	i32 1828773851, ; 150: 0x6d00dfdb => java/security/cert/CertificateFactory
	i32 1851730788, ; 151: 0x6e5f2b64 => java/lang/Runnable
	i32 1859010077, ; 152: 0x6ece3e1d => android/widget/LinearLayout
	i32 1866304377, ; 153: 0x6f3d8b79 => android/view/SearchEvent
	i32 1889248750, ; 154: 0x709ba5ee => java/nio/channels/InterruptibleChannel
	i32 1897013111, ; 155: 0x71121f77 => crc64887e694538fcd9c9/StatsChartView
	i32 1904678085, ; 156: 0x718714c5 => android/text/style/ForegroundColorSpan
	i32 1943778051, ; 157: 0x73dbb303 => android/widget/AdapterView$OnItemSelectedListener
	i32 1944129628, ; 158: 0x73e1105c => java/io/OutputStream
	i32 1950441461, ; 159: 0x74415ff5 => android/text/ParcelableSpan
	i32 1960987726, ; 160: 0x74e24c4e => mono/android/content/DialogInterface_OnDismissListenerImplementor
	i32 1985929388, ; 161: 0x765ee0ac => android/app/Activity
	i32 1987841337, ; 162: 0x767c0d39 => java/lang/Boolean
	i32 1990610968, ; 163: 0x76a65018 => android/widget/AdapterView$OnItemClickListener
	i32 2008064836, ; 164: 0x77b0a344 => android/content/Intent
	i32 2014726135, ; 165: 0x781647f7 => android/view/accessibility/AccessibilityRecord
	i32 2026619833, ; 166: 0x78cbc3b9 => android/widget/FrameLayout$LayoutParams
	i32 2027782872, ; 167: 0x78dd82d8 => android/view/ContextThemeWrapper
	i32 2031450615, ; 168: 0x791579f7 => android/widget/AdapterView
	i32 2036556174, ; 169: 0x7963618e => android/content/DialogInterface
	i32 2057114326, ; 170: 0x7a9d12d6 => java/security/cert/X509Extension
	i32 2061721420, ; 171: 0x7ae35f4c => android/database/CharArrayBuffer
	i32 2064723667, ; 172: 0x7b112ed3 => android/widget/SpinnerAdapter
	i32 2073337312, ; 173: 0x7b949de0 => android/app/AlertDialog$Builder
	i32 2079753938, ; 174: 0x7bf686d2 => android/content/IntentSender
	i32 2080685156, ; 175: 0x7c04bc64 => java/security/SecureRandom
	i32 2090823071, ; 176: 0x7c9f6d9f => android/os/Environment
	i32 2100944957, ; 177: 0x7d39e03d => android/graphics/Path
	i32 2104284586, ; 178: 0x7d6cd5aa => android/content/ClipboardManager
	i32 2114237978, ; 179: 0x7e04b61a => android/content/res/Configuration
	i32 2123880745, ; 180: 0x7e97d929 => android/content/ContentValues
	i32 2183290666, ; 181: 0x82225f2a => mono/android/widget/CompoundButton_OnCheckedChangeListenerImplementor
	i32 2189043474, ; 182: 0x827a2712 => android/graphics/Paint$FontMetrics
	i32 2269094561, ; 183: 0x873fa2a1 => java/net/UnknownServiceException
	i32 2270923754, ; 184: 0x875b8bea => java/net/Proxy$Type
	i32 2284656609, ; 185: 0x882d17e1 => android/app/Application
	i32 2316381801, ; 186: 0x8a112e69 => java/lang/reflect/Type
	i32 2347399792, ; 187: 0x8bea7a70 => android/net/ConnectivityManager
	i32 2363729366, ; 188: 0x8ce3a5d6 => java/lang/Enum
	i32 2371350972, ; 189: 0x8d57f1bc => android/widget/Switch
	i32 2395748977, ; 190: 0x8ecc3a71 => android/view/View$OnLayoutChangeListener
	i32 2411404453, ; 191: 0x8fbb1ca5 => java/lang/UnsupportedOperationException
	i32 2420104680, ; 192: 0x903fdde8 => android/content/res/Resources$Theme
	i32 2443438835, ; 193: 0x91a3eaf3 => java/net/SocketException
	i32 2462006028, ; 194: 0x92bf3b0c => android/content/ComponentCallbacks
	i32 2467524923, ; 195: 0x9313713b => android/window/OnBackInvokedDispatcher
	i32 2520212266, ; 196: 0x9637632a => java/nio/channels/ReadableByteChannel
	i32 2532846927, ; 197: 0x96f82d4f => android/content/SharedPreferences$OnSharedPreferenceChangeListener
	i32 2541780716, ; 198: 0x97807eec => android/view/ContextMenu$ContextMenuInfo
	i32 2558143838, ; 199: 0x987a2d5e => java/io/FileInputStream
	i32 2561967928, ; 200: 0x98b48738 => java/security/cert/X509Certificate
	i32 2594241228, ; 201: 0x9aa0facc => android/widget/BaseAdapter
	i32 2631544208, ; 202: 0x9cda2d90 => android/widget/Filter$FilterListener
	i32 2637159311, ; 203: 0x9d2fdb8f => android/content/pm/PackageManager
	i32 2654672461, ; 204: 0x9e3b164d => java/io/InterruptedIOException
	i32 2663321095, ; 205: 0x9ebf0e07 => mono/java/lang/Runnable
	i32 2664928003, ; 206: 0x9ed79303 => javax/net/ssl/HostnameVerifier
	i32 2681209703, ; 207: 0x9fd00367 => android/widget/Adapter
	i32 2681988174, ; 208: 0x9fdbe44e => android/view/MotionEvent
	i32 2687778660, ; 209: 0xa0343f64 => android/widget/TextView
	i32 2692535938, ; 210: 0xa07cd682 => android/util/Log
	i32 2710910562, ; 211: 0xa1953662 => android/widget/Checkable
	i32 2721599187, ; 212: 0xa2384ed3 => android/graphics/drawable/Drawable
	i32 2731618005, ; 213: 0xa2d12ed5 => android/view/View$MeasureSpec
	i32 2741050037, ; 214: 0xa3611ab5 => java/net/ProxySelector
	i32 2753284754, ; 215: 0xa41bca92 => android/text/style/UpdateAppearance
	i32 2755748727, ; 216: 0xa4416377 => android/text/Spannable
	i32 2762684487, ; 217: 0xa4ab3847 => java/lang/Float
	i32 2815615939, ; 218: 0xa7d2e3c3 => android/os/Build
	i32 2836478263, ; 219: 0xa9113937 => android/widget/ScrollView
	i32 2837435745, ; 220: 0xa91fd561 => android/view/DragEvent
	i32 2855847930, ; 221: 0xaa38c7fa => android/net/wifi/WifiNetworkSpecifier
	i32 2866910344, ; 222: 0xaae19488 => android/view/ActionMode
	i32 2873107855, ; 223: 0xab40258f => android/content/pm/PackageInfo
	i32 2918613155, ; 224: 0xadf680a3 => android/content/DialogInterface$OnClickListener
	i32 2922690929, ; 225: 0xae34b971 => android/graphics/BlendMode
	i32 2932874700, ; 226: 0xaed01dcc => android/view/InputEvent
	i32 2933762856, ; 227: 0xaeddab28 => android/util/AttributeSet
	i32 2942792700, ; 228: 0xaf6773fc => java/lang/Exception
	i32 2944806563, ; 229: 0xaf862ea3 => android/widget/ListView
	i32 2980510762, ; 230: 0xb1a6fc2a => mono/android/runtime/JavaArray
	i32 2983720033, ; 231: 0xb1d7f461 => mono/android/TypeManager
	i32 2983793634, ; 232: 0xb1d913e2 => android/widget/Spinner
	i32 2992653810, ; 233: 0xb26045f2 => crc6475b0d5b881bfcfb5/WifiConnector_LegacyNetworkCallback
	i32 3032808825, ; 234: 0xb4c4fd79 => java/io/StringWriter
	i32 3046187684, ; 235: 0xb59122a4 => crc643c01caa5bd58b444/StatsActivity
	i32 3072461607, ; 236: 0xb7220b27 => java/util/concurrent/Future
	i32 3087255038, ; 237: 0xb803c5fe => android/preference/PreferenceManager
	i32 3151193661, ; 238: 0xbbd3663d => crc64d4bf71c1e765891b/SyncToPcActivity
	i32 3183271055, ; 239: 0xbdbcdc8f => android/view/ActionMode$Callback
	i32 3203363508, ; 240: 0xbeef72b4 => android/view/KeyboardShortcutGroup
	i32 3214744068, ; 241: 0xbf9d1a04 => android/view/WindowManager
	i32 3238443669, ; 242: 0xc106ba95 => android/widget/RadioGroup$OnCheckedChangeListener
	i32 3264154243, ; 243: 0xc28f0a83 => java/io/Flushable
	i32 3271087717, ; 244: 0xc2f8d665 => mono/android/view/View_OnLayoutChangeListenerImplementor
	i32 3281925794, ; 245: 0xc39e36a2 => android/view/MenuItem
	i32 3300906352, ; 246: 0xc4bfd570 => javax/net/ssl/SSLSession
	i32 3319735188, ; 247: 0xc5df2394 => java/net/Proxy
	i32 3367348199, ; 248: 0xc8b5a7e7 => mono/android/widget/RadioGroup_OnCheckedChangeListenerImplementor
	i32 3379688415, ; 249: 0xc971f3df => android/text/Editable
	i32 3386853318, ; 250: 0xc9df47c6 => androidx/core/content/pm/PackageInfoCompat
	i32 3397817114, ; 251: 0xca86931a => android/widget/ArrayAdapter
	i32 3409419575, ; 252: 0xcb379d37 => javax/net/ssl/HttpsURLConnection
	i32 3423467887, ; 253: 0xcc0df96f => java/lang/Number
	i32 3427035968, ; 254: 0xcc446b40 => xamarin/android/net/OldAndroidSSLSocketFactory
	i32 3430868172, ; 255: 0xcc7ee4cc => android/content/SharedPreferences
	i32 3519931621, ; 256: 0xd1cde4e5 => java/net/URLConnection
	i32 3549151004, ; 257: 0xd38bbf1c => android/provider/Settings
	i32 3576242387, ; 258: 0xd52920d3 => android/runtime/JavaProxyThrowable
	i32 3581618783, ; 259: 0xd57b2a5f => android/provider/MediaStore$Downloads
	i32 3590909812, ; 260: 0xd608ef74 => com/google/common/util/concurrent/ListenableFuture
	i32 3597654493, ; 261: 0xd66fd9dd => android/widget/AbsListView
	i32 3665774669, ; 262: 0xda7f484d => android/view/LayoutInflater
	i32 3666243682, ; 263: 0xda867062 => java/lang/String
	i32 3669061717, ; 264: 0xdab17055 => java/net/InetSocketAddress
	i32 3673444347, ; 265: 0xdaf44ffb => android/view/accessibility/AccessibilityEvent
	i32 3683323802, ; 266: 0xdb8b0f9a => mono/android/runtime/JavaObject
	i32 3684070586, ; 267: 0xdb9674ba => android/view/ActionProvider
	i32 3698769169, ; 268: 0xdc76bd11 => android/text/SpannableStringBuilder
	i32 3701331277, ; 269: 0xdc9dd54d => android/graphics/Paint$Style
	i32 3702230909, ; 270: 0xdcab8f7d => java/lang/Double
	i32 3702422870, ; 271: 0xdcae7d56 => android/view/ViewTreeObserver$OnPreDrawListener
	i32 3715861037, ; 272: 0xdd7b8a2d => android/os/Build$VERSION
	i32 3721088312, ; 273: 0xddcb4d38 => android/text/NoCopySpan
	i32 3722843854, ; 274: 0xdde616ce => javax/net/SocketFactory
	i32 3726680736, ; 275: 0xde20a2a0 => java/net/ProtocolException
	i32 3746020715, ; 276: 0xdf47bd6b => android/graphics/drawable/Drawable$Callback
	i32 3763853270, ; 277: 0xe057d7d6 => android/view/Window
	i32 3776101439, ; 278: 0xe112bc3f => java/util/BitSet
	i32 3823421666, ; 279: 0xe3e4c8e2 => android/net/Uri
	i32 3828108282, ; 280: 0xe42c4bfa => android/widget/TextView$BufferType
	i32 3846932217, ; 281: 0xe54b86f9 => javax/net/ssl/X509TrustManager
	i32 3882570516, ; 282: 0xe76b5314 => java/lang/Class
	i32 3893629743, ; 283: 0xe814132f => android/view/LayoutInflater$Factory2
	i32 3895425567, ; 284: 0xe82f7a1f => androidx/core/app/SharedElementCallback
	i32 3900328001, ; 285: 0xe87a4841 => android/graphics/Typeface
	i32 3900581163, ; 286: 0xe87e252b => java/io/InputStream
	i32 3901837667, ; 287: 0xe8915163 => android/text/InputFilter
	i32 3912451735, ; 288: 0xe9334697 => java/security/KeyStore
	i32 3919758710, ; 289: 0xe9a2c576 => android/view/ContextMenu
	i32 3920921908, ; 290: 0xe9b48534 => android/net/NetworkCapabilities
	i32 3931120197, ; 291: 0xea502245 => mono/android/content/DialogInterface_OnClickListenerImplementor
	i32 3933245259, ; 292: 0xea708f4b => android/graphics/Rect
	i32 3960999444, ; 293: 0xec180e14 => android/widget/Toast
	i32 3969984744, ; 294: 0xeca128e8 => mono/android/runtime/InputStreamAdapter
	i32 3975001277, ; 295: 0xecedb4bd => javax/net/ssl/SSLSocketFactory
	i32 3993327007, ; 296: 0xee05559f => android/content/ContextWrapper
	i32 3995406185, ; 297: 0xee250f69 => android/graphics/Canvas
	i32 4020308495, ; 298: 0xefa10a0f => java/lang/Error
	i32 4026153166, ; 299: 0xeffa38ce => androidx/core/view/DragAndDropPermissionsCompat
	i32 4030673356, ; 300: 0xf03f31cc => android/app/Dialog
	i32 4044525863, ; 301: 0xf1129127 => android/content/ComponentCallbacks2
	i32 4051772911, ; 302: 0xf18125ef => android/content/Context
	i32 4056674536, ; 303: 0xf1cbf0e8 => java/lang/NoClassDefFoundError
	i32 4076210344, ; 304: 0xf2f608a8 => crc64d4bf71c1e765891b/MainActivity
	i32 4089459037, ; 305: 0xf3c0315d => java/nio/Buffer
	i32 4098107575, ; 306: 0xf44428b7 => mono/android/view/View_OnClickListenerImplementor
	i32 4101363546, ; 307: 0xf475d75a => java/io/Writer
	i32 4118878202, ; 308: 0xf58117fa => android/os/Looper
	i32 4127266501, ; 309: 0xf60116c5 => mono/android/widget/AdapterView_OnItemClickListenerImplementor
	i32 4136260101, ; 310: 0xf68a5205 => android/text/ClipboardManager
	i32 4148593869, ; 311: 0xf74684cd => javax/net/ssl/TrustManagerFactory
	i32 4157808693, ; 312: 0xf7d32035 => java/io/IOException
	i32 4166165970, ; 313: 0xf852a5d2 => android/text/TextWatcher
	i32 4175389061, ; 314: 0xf8df6185 => android/content/ContentUris
	i32 4232707919, ; 315: 0xfc49ff4f => java/util/HashSet
	i32 4236355936, ; 316: 0xfc81a960 => android/view/ViewTreeObserver$OnGlobalLayoutListener
	i32 4236724582, ; 317: 0xfc874966 => android/os/Parcelable
	i32 4237386260, ; 318: 0xfc916214 => android/view/MenuItem$OnMenuItemClickListener
	i32 4248811056, ; 319: 0xfd3fb630 => android/view/Menu
	i32 4264406764, ; 320: 0xfe2daeec => android/net/NetworkSpecifier
	i32 4271127433, ; 321: 0xfe943b89 => android/graphics/PorterDuff
	i32 4277523103, ; 322: 0xfef5d29f => java/io/Closeable
	i32 4278949669 ; 323: 0xff0b9725 => android/widget/CompoundButton$OnCheckedChangeListener
], align 4

; java_type_names
@__java_type_names.0 = internal constant [47 x i8] c"xamarin/android/net/OldAndroidSSLSocketFactory\00", align 1
@__java_type_names.1 = internal constant [32 x i8] c"javax/security/cert/Certificate\00", align 1
@__java_type_names.2 = internal constant [36 x i8] c"javax/security/cert/X509Certificate\00", align 1
@__java_type_names.3 = internal constant [28 x i8] c"javax/security/auth/Subject\00", align 1
@__java_type_names.4 = internal constant [24 x i8] c"javax/net/SocketFactory\00", align 1
@__java_type_names.5 = internal constant [33 x i8] c"javax/net/ssl/HttpsURLConnection\00", align 1
@__java_type_names.6 = internal constant [31 x i8] c"javax/net/ssl/HostnameVerifier\00", align 1
@__java_type_names.7 = internal constant [25 x i8] c"javax/net/ssl/KeyManager\00", align 1
@__java_type_names.8 = internal constant [25 x i8] c"javax/net/ssl/SSLSession\00", align 1
@__java_type_names.9 = internal constant [32 x i8] c"javax/net/ssl/SSLSessionContext\00", align 1
@__java_type_names.10 = internal constant [27 x i8] c"javax/net/ssl/TrustManager\00", align 1
@__java_type_names.11 = internal constant [31 x i8] c"javax/net/ssl/X509TrustManager\00", align 1
@__java_type_names.12 = internal constant [32 x i8] c"javax/net/ssl/KeyManagerFactory\00", align 1
@__java_type_names.13 = internal constant [25 x i8] c"javax/net/ssl/SSLContext\00", align 1
@__java_type_names.14 = internal constant [31 x i8] c"javax/net/ssl/SSLSocketFactory\00", align 1
@__java_type_names.15 = internal constant [34 x i8] c"javax/net/ssl/TrustManagerFactory\00", align 1
@__java_type_names.16 = internal constant [37 x i8] c"android/window/OnBackInvokedCallback\00", align 1
@__java_type_names.17 = internal constant [39 x i8] c"android/window/OnBackInvokedDispatcher\00", align 1
@__java_type_names.18 = internal constant [27 x i8] c"android/widget/AbsListView\00", align 1
@__java_type_names.19 = internal constant [26 x i8] c"android/widget/AbsSpinner\00", align 1
@__java_type_names.20 = internal constant [27 x i8] c"android/widget/AdapterView\00", align 1
@__java_type_names.21 = internal constant [47 x i8] c"android/widget/AdapterView$OnItemClickListener\00", align 1
@__java_type_names.22 = internal constant [63 x i8] c"mono/android/widget/AdapterView_OnItemClickListenerImplementor\00", align 1
@__java_type_names.23 = internal constant [50 x i8] c"android/widget/AdapterView$OnItemSelectedListener\00", align 1
@__java_type_names.24 = internal constant [66 x i8] c"mono/android/widget/AdapterView_OnItemSelectedListenerImplementor\00", align 1
@__java_type_names.25 = internal constant [28 x i8] c"android/widget/ArrayAdapter\00", align 1
@__java_type_names.26 = internal constant [27 x i8] c"android/widget/BaseAdapter\00", align 1
@__java_type_names.27 = internal constant [22 x i8] c"android/widget/Button\00", align 1
@__java_type_names.28 = internal constant [30 x i8] c"android/widget/CompoundButton\00", align 1
@__java_type_names.29 = internal constant [54 x i8] c"android/widget/CompoundButton$OnCheckedChangeListener\00", align 1
@__java_type_names.30 = internal constant [70 x i8] c"mono/android/widget/CompoundButton_OnCheckedChangeListenerImplementor\00", align 1
@__java_type_names.31 = internal constant [24 x i8] c"android/widget/EditText\00", align 1
@__java_type_names.32 = internal constant [22 x i8] c"android/widget/Filter\00", align 1
@__java_type_names.33 = internal constant [37 x i8] c"android/widget/Filter$FilterListener\00", align 1
@__java_type_names.34 = internal constant [27 x i8] c"android/widget/FrameLayout\00", align 1
@__java_type_names.35 = internal constant [40 x i8] c"android/widget/FrameLayout$LayoutParams\00", align 1
@__java_type_names.36 = internal constant [36 x i8] c"android/widget/HorizontalScrollView\00", align 1
@__java_type_names.37 = internal constant [23 x i8] c"android/widget/Adapter\00", align 1
@__java_type_names.38 = internal constant [25 x i8] c"android/widget/Checkable\00", align 1
@__java_type_names.39 = internal constant [26 x i8] c"android/widget/Filterable\00", align 1
@__java_type_names.40 = internal constant [27 x i8] c"android/widget/ListAdapter\00", align 1
@__java_type_names.41 = internal constant [30 x i8] c"android/widget/SpinnerAdapter\00", align 1
@__java_type_names.42 = internal constant [36 x i8] c"android/widget/ThemedSpinnerAdapter\00", align 1
@__java_type_names.43 = internal constant [28 x i8] c"android/widget/LinearLayout\00", align 1
@__java_type_names.44 = internal constant [41 x i8] c"android/widget/LinearLayout$LayoutParams\00", align 1
@__java_type_names.45 = internal constant [24 x i8] c"android/widget/ListView\00", align 1
@__java_type_names.46 = internal constant [27 x i8] c"android/widget/ProgressBar\00", align 1
@__java_type_names.47 = internal constant [27 x i8] c"android/widget/RadioButton\00", align 1
@__java_type_names.48 = internal constant [26 x i8] c"android/widget/RadioGroup\00", align 1
@__java_type_names.49 = internal constant [50 x i8] c"android/widget/RadioGroup$OnCheckedChangeListener\00", align 1
@__java_type_names.50 = internal constant [66 x i8] c"mono/android/widget/RadioGroup_OnCheckedChangeListenerImplementor\00", align 1
@__java_type_names.51 = internal constant [26 x i8] c"android/widget/ScrollView\00", align 1
@__java_type_names.52 = internal constant [23 x i8] c"android/widget/Spinner\00", align 1
@__java_type_names.53 = internal constant [22 x i8] c"android/widget/Switch\00", align 1
@__java_type_names.54 = internal constant [24 x i8] c"android/widget/TextView\00", align 1
@__java_type_names.55 = internal constant [35 x i8] c"android/widget/TextView$BufferType\00", align 1
@__java_type_names.56 = internal constant [21 x i8] c"android/widget/Toast\00", align 1
@__java_type_names.57 = internal constant [24 x i8] c"android/view/ActionMode\00", align 1
@__java_type_names.58 = internal constant [33 x i8] c"android/view/ActionMode$Callback\00", align 1
@__java_type_names.59 = internal constant [28 x i8] c"android/view/ActionProvider\00", align 1
@__java_type_names.60 = internal constant [33 x i8] c"android/view/ContextThemeWrapper\00", align 1
@__java_type_names.61 = internal constant [21 x i8] c"android/view/Display\00", align 1
@__java_type_names.62 = internal constant [23 x i8] c"android/view/DragEvent\00", align 1
@__java_type_names.63 = internal constant [41 x i8] c"android/view/ContextMenu$ContextMenuInfo\00", align 1
@__java_type_names.64 = internal constant [25 x i8] c"android/view/ContextMenu\00", align 1
@__java_type_names.65 = internal constant [18 x i8] c"android/view/Menu\00", align 1
@__java_type_names.66 = internal constant [45 x i8] c"android/view/MenuItem$OnActionExpandListener\00", align 1
@__java_type_names.67 = internal constant [46 x i8] c"android/view/MenuItem$OnMenuItemClickListener\00", align 1
@__java_type_names.68 = internal constant [22 x i8] c"android/view/MenuItem\00", align 1
@__java_type_names.69 = internal constant [24 x i8] c"android/view/InputEvent\00", align 1
@__java_type_names.70 = internal constant [21 x i8] c"android/view/SubMenu\00", align 1
@__java_type_names.71 = internal constant [25 x i8] c"android/view/ViewManager\00", align 1
@__java_type_names.72 = internal constant [24 x i8] c"android/view/ViewParent\00", align 1
@__java_type_names.73 = internal constant [40 x i8] c"android/view/WindowManager$LayoutParams\00", align 1
@__java_type_names.74 = internal constant [27 x i8] c"android/view/WindowManager\00", align 1
@__java_type_names.75 = internal constant [35 x i8] c"android/view/KeyboardShortcutGroup\00", align 1
@__java_type_names.76 = internal constant [22 x i8] c"android/view/KeyEvent\00", align 1
@__java_type_names.77 = internal constant [31 x i8] c"android/view/KeyEvent$Callback\00", align 1
@__java_type_names.78 = internal constant [28 x i8] c"android/view/LayoutInflater\00", align 1
@__java_type_names.79 = internal constant [36 x i8] c"android/view/LayoutInflater$Factory\00", align 1
@__java_type_names.80 = internal constant [37 x i8] c"android/view/LayoutInflater$Factory2\00", align 1
@__java_type_names.81 = internal constant [25 x i8] c"android/view/MotionEvent\00", align 1
@__java_type_names.82 = internal constant [25 x i8] c"android/view/SearchEvent\00", align 1
@__java_type_names.83 = internal constant [18 x i8] c"android/view/View\00", align 1
@__java_type_names.84 = internal constant [30 x i8] c"android/view/View$MeasureSpec\00", align 1
@__java_type_names.85 = internal constant [34 x i8] c"android/view/View$OnClickListener\00", align 1
@__java_type_names.86 = internal constant [50 x i8] c"mono/android/view/View_OnClickListenerImplementor\00", align 1
@__java_type_names.87 = internal constant [46 x i8] c"android/view/View$OnCreateContextMenuListener\00", align 1
@__java_type_names.88 = internal constant [41 x i8] c"android/view/View$OnLayoutChangeListener\00", align 1
@__java_type_names.89 = internal constant [57 x i8] c"mono/android/view/View_OnLayoutChangeListenerImplementor\00", align 1
@__java_type_names.90 = internal constant [23 x i8] c"android/view/ViewGroup\00", align 1
@__java_type_names.91 = internal constant [36 x i8] c"android/view/ViewGroup$LayoutParams\00", align 1
@__java_type_names.92 = internal constant [42 x i8] c"android/view/ViewGroup$MarginLayoutParams\00", align 1
@__java_type_names.93 = internal constant [30 x i8] c"android/view/ViewTreeObserver\00", align 1
@__java_type_names.94 = internal constant [53 x i8] c"android/view/ViewTreeObserver$OnGlobalLayoutListener\00", align 1
@__java_type_names.95 = internal constant [48 x i8] c"android/view/ViewTreeObserver$OnPreDrawListener\00", align 1
@__java_type_names.96 = internal constant [56 x i8] c"android/view/ViewTreeObserver$OnTouchModeChangeListener\00", align 1
@__java_type_names.97 = internal constant [20 x i8] c"android/view/Window\00", align 1
@__java_type_names.98 = internal constant [29 x i8] c"android/view/Window$Callback\00", align 1
@__java_type_names.99 = internal constant [27 x i8] c"android/view/WindowMetrics\00", align 1
@__java_type_names.100 = internal constant [46 x i8] c"android/view/accessibility/AccessibilityEvent\00", align 1
@__java_type_names.101 = internal constant [47 x i8] c"android/view/accessibility/AccessibilityRecord\00", align 1
@__java_type_names.102 = internal constant [52 x i8] c"android/view/accessibility/AccessibilityEventSource\00", align 1
@__java_type_names.103 = internal constant [28 x i8] c"android/util/DisplayMetrics\00", align 1
@__java_type_names.104 = internal constant [26 x i8] c"android/util/AttributeSet\00", align 1
@__java_type_names.105 = internal constant [17 x i8] c"android/util/Log\00", align 1
@__java_type_names.106 = internal constant [30 x i8] c"android/text/ClipboardManager\00", align 1
@__java_type_names.107 = internal constant [22 x i8] c"android/text/Editable\00", align 1
@__java_type_names.108 = internal constant [22 x i8] c"android/text/GetChars\00", align 1
@__java_type_names.109 = internal constant [25 x i8] c"android/text/InputFilter\00", align 1
@__java_type_names.110 = internal constant [24 x i8] c"android/text/NoCopySpan\00", align 1
@__java_type_names.111 = internal constant [28 x i8] c"android/text/ParcelableSpan\00", align 1
@__java_type_names.112 = internal constant [23 x i8] c"android/text/Spannable\00", align 1
@__java_type_names.113 = internal constant [21 x i8] c"android/text/Spanned\00", align 1
@__java_type_names.114 = internal constant [25 x i8] c"android/text/TextWatcher\00", align 1
@__java_type_names.115 = internal constant [36 x i8] c"android/text/SpannableStringBuilder\00", align 1
@__java_type_names.116 = internal constant [34 x i8] c"android/text/style/CharacterStyle\00", align 1
@__java_type_names.117 = internal constant [39 x i8] c"android/text/style/ForegroundColorSpan\00", align 1
@__java_type_names.118 = internal constant [36 x i8] c"android/text/style/UpdateAppearance\00", align 1
@__java_type_names.119 = internal constant [28 x i8] c"android/provider/MediaStore\00", align 1
@__java_type_names.120 = internal constant [38 x i8] c"android/provider/MediaStore$Downloads\00", align 1
@__java_type_names.121 = internal constant [26 x i8] c"android/provider/Settings\00", align 1
@__java_type_names.122 = internal constant [41 x i8] c"android/provider/Settings$NameValueTable\00", align 1
@__java_type_names.123 = internal constant [33 x i8] c"android/provider/Settings$Secure\00", align 1
@__java_type_names.124 = internal constant [37 x i8] c"android/preference/PreferenceManager\00", align 1
@__java_type_names.125 = internal constant [22 x i8] c"android/os/BaseBundle\00", align 1
@__java_type_names.126 = internal constant [17 x i8] c"android/os/Build\00", align 1
@__java_type_names.127 = internal constant [25 x i8] c"android/os/Build$VERSION\00", align 1
@__java_type_names.128 = internal constant [18 x i8] c"android/os/Bundle\00", align 1
@__java_type_names.129 = internal constant [23 x i8] c"android/os/Environment\00", align 1
@__java_type_names.130 = internal constant [19 x i8] c"android/os/Handler\00", align 1
@__java_type_names.131 = internal constant [22 x i8] c"android/os/Parcelable\00", align 1
@__java_type_names.132 = internal constant [18 x i8] c"android/os/Looper\00", align 1
@__java_type_names.133 = internal constant [18 x i8] c"android/os/Parcel\00", align 1
@__java_type_names.134 = internal constant [32 x i8] c"android/net/ConnectivityManager\00", align 1
@__java_type_names.135 = internal constant [48 x i8] c"android/net/ConnectivityManager$NetworkCallback\00", align 1
@__java_type_names.136 = internal constant [20 x i8] c"android/net/Network\00", align 1
@__java_type_names.137 = internal constant [32 x i8] c"android/net/NetworkCapabilities\00", align 1
@__java_type_names.138 = internal constant [27 x i8] c"android/net/NetworkRequest\00", align 1
@__java_type_names.139 = internal constant [35 x i8] c"android/net/NetworkRequest$Builder\00", align 1
@__java_type_names.140 = internal constant [29 x i8] c"android/net/NetworkSpecifier\00", align 1
@__java_type_names.141 = internal constant [16 x i8] c"android/net/Uri\00", align 1
@__java_type_names.142 = internal constant [35 x i8] c"android/net/wifi/WifiConfiguration\00", align 1
@__java_type_names.143 = internal constant [29 x i8] c"android/net/wifi/WifiManager\00", align 1
@__java_type_names.144 = internal constant [38 x i8] c"android/net/wifi/WifiNetworkSpecifier\00", align 1
@__java_type_names.145 = internal constant [46 x i8] c"android/net/wifi/WifiNetworkSpecifier$Builder\00", align 1
@__java_type_names.146 = internal constant [27 x i8] c"android/graphics/BlendMode\00", align 1
@__java_type_names.147 = internal constant [24 x i8] c"android/graphics/Canvas\00", align 1
@__java_type_names.148 = internal constant [23 x i8] c"android/graphics/Color\00", align 1
@__java_type_names.149 = internal constant [24 x i8] c"android/graphics/Matrix\00", align 1
@__java_type_names.150 = internal constant [23 x i8] c"android/graphics/Paint\00", align 1
@__java_type_names.151 = internal constant [35 x i8] c"android/graphics/Paint$FontMetrics\00", align 1
@__java_type_names.152 = internal constant [29 x i8] c"android/graphics/Paint$Style\00", align 1
@__java_type_names.153 = internal constant [22 x i8] c"android/graphics/Path\00", align 1
@__java_type_names.154 = internal constant [23 x i8] c"android/graphics/Point\00", align 1
@__java_type_names.155 = internal constant [24 x i8] c"android/graphics/PointF\00", align 1
@__java_type_names.156 = internal constant [28 x i8] c"android/graphics/PorterDuff\00", align 1
@__java_type_names.157 = internal constant [33 x i8] c"android/graphics/PorterDuff$Mode\00", align 1
@__java_type_names.158 = internal constant [22 x i8] c"android/graphics/Rect\00", align 1
@__java_type_names.159 = internal constant [23 x i8] c"android/graphics/RectF\00", align 1
@__java_type_names.160 = internal constant [26 x i8] c"android/graphics/Typeface\00", align 1
@__java_type_names.161 = internal constant [35 x i8] c"android/graphics/drawable/Drawable\00", align 1
@__java_type_names.162 = internal constant [44 x i8] c"android/graphics/drawable/Drawable$Callback\00", align 1
@__java_type_names.163 = internal constant [33 x i8] c"android/database/CharArrayBuffer\00", align 1
@__java_type_names.164 = internal constant [33 x i8] c"android/database/ContentObserver\00", align 1
@__java_type_names.165 = internal constant [33 x i8] c"android/database/DataSetObserver\00", align 1
@__java_type_names.166 = internal constant [24 x i8] c"android/database/Cursor\00", align 1
@__java_type_names.167 = internal constant [21 x i8] c"android/app/Activity\00", align 1
@__java_type_names.168 = internal constant [24 x i8] c"android/app/AlertDialog\00", align 1
@__java_type_names.169 = internal constant [32 x i8] c"android/app/AlertDialog$Builder\00", align 1
@__java_type_names.170 = internal constant [24 x i8] c"android/app/Application\00", align 1
@__java_type_names.171 = internal constant [19 x i8] c"android/app/Dialog\00", align 1
@__java_type_names.172 = internal constant [33 x i8] c"android/content/ClipboardManager\00", align 1
@__java_type_names.173 = internal constant [25 x i8] c"android/content/ClipData\00", align 1
@__java_type_names.174 = internal constant [30 x i8] c"android/content/ComponentName\00", align 1
@__java_type_names.175 = internal constant [32 x i8] c"android/content/ContentProvider\00", align 1
@__java_type_names.176 = internal constant [32 x i8] c"android/content/ContentResolver\00", align 1
@__java_type_names.177 = internal constant [28 x i8] c"android/content/ContentUris\00", align 1
@__java_type_names.178 = internal constant [30 x i8] c"android/content/ContentValues\00", align 1
@__java_type_names.179 = internal constant [24 x i8] c"android/content/Context\00", align 1
@__java_type_names.180 = internal constant [31 x i8] c"android/content/ContextWrapper\00", align 1
@__java_type_names.181 = internal constant [35 x i8] c"android/content/ComponentCallbacks\00", align 1
@__java_type_names.182 = internal constant [36 x i8] c"android/content/ComponentCallbacks2\00", align 1
@__java_type_names.183 = internal constant [48 x i8] c"android/content/DialogInterface$OnClickListener\00", align 1
@__java_type_names.184 = internal constant [64 x i8] c"mono/android/content/DialogInterface_OnClickListenerImplementor\00", align 1
@__java_type_names.185 = internal constant [50 x i8] c"android/content/DialogInterface$OnDismissListener\00", align 1
@__java_type_names.186 = internal constant [66 x i8] c"mono/android/content/DialogInterface_OnDismissListenerImplementor\00", align 1
@__java_type_names.187 = internal constant [32 x i8] c"android/content/DialogInterface\00", align 1
@__java_type_names.188 = internal constant [23 x i8] c"android/content/Intent\00", align 1
@__java_type_names.189 = internal constant [29 x i8] c"android/content/IntentSender\00", align 1
@__java_type_names.190 = internal constant [41 x i8] c"android/content/SharedPreferences$Editor\00", align 1
@__java_type_names.191 = internal constant [67 x i8] c"android/content/SharedPreferences$OnSharedPreferenceChangeListener\00", align 1
@__java_type_names.192 = internal constant [34 x i8] c"android/content/SharedPreferences\00", align 1
@__java_type_names.193 = internal constant [35 x i8] c"android/content/res/ColorStateList\00", align 1
@__java_type_names.194 = internal constant [34 x i8] c"android/content/res/Configuration\00", align 1
@__java_type_names.195 = internal constant [30 x i8] c"android/content/res/Resources\00", align 1
@__java_type_names.196 = internal constant [36 x i8] c"android/content/res/Resources$Theme\00", align 1
@__java_type_names.197 = internal constant [31 x i8] c"android/content/pm/PackageInfo\00", align 1
@__java_type_names.198 = internal constant [34 x i8] c"android/content/pm/PackageManager\00", align 1
@__java_type_names.199 = internal constant [40 x i8] c"mono/android/runtime/InputStreamAdapter\00", align 1
@__java_type_names.200 = internal constant [31 x i8] c"mono/android/runtime/JavaArray\00", align 1
@__java_type_names.201 = internal constant [21 x i8] c"java/util/Collection\00", align 1
@__java_type_names.202 = internal constant [18 x i8] c"java/util/HashMap\00", align 1
@__java_type_names.203 = internal constant [20 x i8] c"java/util/ArrayList\00", align 1
@__java_type_names.204 = internal constant [32 x i8] c"mono/android/runtime/JavaObject\00", align 1
@__java_type_names.205 = internal constant [35 x i8] c"android/runtime/JavaProxyThrowable\00", align 1
@__java_type_names.206 = internal constant [18 x i8] c"java/util/HashSet\00", align 1
@__java_type_names.207 = internal constant [41 x i8] c"mono/android/runtime/OutputStreamAdapter\00", align 1
@__java_type_names.208 = internal constant [17 x i8] c"java/util/BitSet\00", align 1
@__java_type_names.209 = internal constant [22 x i8] c"java/util/Enumeration\00", align 1
@__java_type_names.210 = internal constant [19 x i8] c"java/util/Iterator\00", align 1
@__java_type_names.211 = internal constant [17 x i8] c"java/util/Random\00", align 1
@__java_type_names.212 = internal constant [28 x i8] c"java/util/function/Consumer\00", align 1
@__java_type_names.213 = internal constant [30 x i8] c"java/util/concurrent/Executor\00", align 1
@__java_type_names.214 = internal constant [28 x i8] c"java/util/concurrent/Future\00", align 1
@__java_type_names.215 = internal constant [30 x i8] c"java/util/concurrent/TimeUnit\00", align 1
@__java_type_names.216 = internal constant [24 x i8] c"java/security/Principal\00", align 1
@__java_type_names.217 = internal constant [23 x i8] c"java/security/KeyStore\00", align 1
@__java_type_names.218 = internal constant [42 x i8] c"java/security/KeyStore$LoadStoreParameter\00", align 1
@__java_type_names.219 = internal constant [43 x i8] c"java/security/KeyStore$ProtectionParameter\00", align 1
@__java_type_names.220 = internal constant [27 x i8] c"java/security/SecureRandom\00", align 1
@__java_type_names.221 = internal constant [31 x i8] c"java/security/cert/Certificate\00", align 1
@__java_type_names.222 = internal constant [38 x i8] c"java/security/cert/CertificateFactory\00", align 1
@__java_type_names.223 = internal constant [33 x i8] c"java/security/cert/X509Extension\00", align 1
@__java_type_names.224 = internal constant [35 x i8] c"java/security/cert/X509Certificate\00", align 1
@__java_type_names.225 = internal constant [16 x i8] c"java/nio/Buffer\00", align 1
@__java_type_names.226 = internal constant [20 x i8] c"java/nio/ByteBuffer\00", align 1
@__java_type_names.227 = internal constant [30 x i8] c"java/nio/channels/FileChannel\00", align 1
@__java_type_names.228 = internal constant [30 x i8] c"java/nio/channels/ByteChannel\00", align 1
@__java_type_names.229 = internal constant [26 x i8] c"java/nio/channels/Channel\00", align 1
@__java_type_names.230 = internal constant [39 x i8] c"java/nio/channels/GatheringByteChannel\00", align 1
@__java_type_names.231 = internal constant [39 x i8] c"java/nio/channels/InterruptibleChannel\00", align 1
@__java_type_names.232 = internal constant [38 x i8] c"java/nio/channels/ReadableByteChannel\00", align 1
@__java_type_names.233 = internal constant [40 x i8] c"java/nio/channels/ScatteringByteChannel\00", align 1
@__java_type_names.234 = internal constant [38 x i8] c"java/nio/channels/SeekableByteChannel\00", align 1
@__java_type_names.235 = internal constant [38 x i8] c"java/nio/channels/WritableByteChannel\00", align 1
@__java_type_names.236 = internal constant [51 x i8] c"java/nio/channels/spi/AbstractInterruptibleChannel\00", align 1
@__java_type_names.237 = internal constant [26 x i8] c"java/net/ConnectException\00", align 1
@__java_type_names.238 = internal constant [27 x i8] c"java/net/HttpURLConnection\00", align 1
@__java_type_names.239 = internal constant [27 x i8] c"java/net/InetSocketAddress\00", align 1
@__java_type_names.240 = internal constant [27 x i8] c"java/net/ProtocolException\00", align 1
@__java_type_names.241 = internal constant [15 x i8] c"java/net/Proxy\00", align 1
@__java_type_names.242 = internal constant [20 x i8] c"java/net/Proxy$Type\00", align 1
@__java_type_names.243 = internal constant [23 x i8] c"java/net/ProxySelector\00", align 1
@__java_type_names.244 = internal constant [23 x i8] c"java/net/SocketAddress\00", align 1
@__java_type_names.245 = internal constant [25 x i8] c"java/net/SocketException\00", align 1
@__java_type_names.246 = internal constant [32 x i8] c"java/net/SocketTimeoutException\00", align 1
@__java_type_names.247 = internal constant [33 x i8] c"java/net/UnknownServiceException\00", align 1
@__java_type_names.248 = internal constant [13 x i8] c"java/net/URI\00", align 1
@__java_type_names.249 = internal constant [13 x i8] c"java/net/URL\00", align 1
@__java_type_names.250 = internal constant [23 x i8] c"java/net/URLConnection\00", align 1
@__java_type_names.251 = internal constant [18 x i8] c"java/lang/Boolean\00", align 1
@__java_type_names.252 = internal constant [15 x i8] c"java/lang/Byte\00", align 1
@__java_type_names.253 = internal constant [20 x i8] c"java/lang/Character\00", align 1
@__java_type_names.254 = internal constant [16 x i8] c"java/lang/Class\00", align 1
@__java_type_names.255 = internal constant [29 x i8] c"java/lang/ClassCastException\00", align 1
@__java_type_names.256 = internal constant [33 x i8] c"java/lang/ClassNotFoundException\00", align 1
@__java_type_names.257 = internal constant [17 x i8] c"java/lang/Double\00", align 1
@__java_type_names.258 = internal constant [15 x i8] c"java/lang/Enum\00", align 1
@__java_type_names.259 = internal constant [16 x i8] c"java/lang/Error\00", align 1
@__java_type_names.260 = internal constant [20 x i8] c"java/lang/Exception\00", align 1
@__java_type_names.261 = internal constant [16 x i8] c"java/lang/Float\00", align 1
@__java_type_names.262 = internal constant [21 x i8] c"java/lang/Appendable\00", align 1
@__java_type_names.263 = internal constant [23 x i8] c"java/lang/CharSequence\00", align 1
@__java_type_names.264 = internal constant [20 x i8] c"java/lang/Cloneable\00", align 1
@__java_type_names.265 = internal constant [21 x i8] c"java/lang/Comparable\00", align 1
@__java_type_names.266 = internal constant [35 x i8] c"java/lang/IllegalArgumentException\00", align 1
@__java_type_names.267 = internal constant [32 x i8] c"java/lang/IllegalStateException\00", align 1
@__java_type_names.268 = internal constant [36 x i8] c"java/lang/IndexOutOfBoundsException\00", align 1
@__java_type_names.269 = internal constant [18 x i8] c"java/lang/Integer\00", align 1
@__java_type_names.270 = internal constant [19 x i8] c"java/lang/Runnable\00", align 1
@__java_type_names.271 = internal constant [23 x i8] c"java/lang/LinkageError\00", align 1
@__java_type_names.272 = internal constant [15 x i8] c"java/lang/Long\00", align 1
@__java_type_names.273 = internal constant [31 x i8] c"java/lang/NoClassDefFoundError\00", align 1
@__java_type_names.274 = internal constant [31 x i8] c"java/lang/NullPointerException\00", align 1
@__java_type_names.275 = internal constant [17 x i8] c"java/lang/Number\00", align 1
@__java_type_names.276 = internal constant [17 x i8] c"java/lang/Object\00", align 1
@__java_type_names.277 = internal constant [39 x i8] c"java/lang/ReflectiveOperationException\00", align 1
@__java_type_names.278 = internal constant [27 x i8] c"java/lang/RuntimeException\00", align 1
@__java_type_names.279 = internal constant [28 x i8] c"java/lang/SecurityException\00", align 1
@__java_type_names.280 = internal constant [16 x i8] c"java/lang/Short\00", align 1
@__java_type_names.281 = internal constant [17 x i8] c"java/lang/String\00", align 1
@__java_type_names.282 = internal constant [17 x i8] c"java/lang/Thread\00", align 1
@__java_type_names.283 = internal constant [35 x i8] c"mono/java/lang/RunnableImplementor\00", align 1
@__java_type_names.284 = internal constant [20 x i8] c"java/lang/Throwable\00", align 1
@__java_type_names.285 = internal constant [40 x i8] c"java/lang/UnsupportedOperationException\00", align 1
@__java_type_names.286 = internal constant [24 x i8] c"mono/java/lang/Runnable\00", align 1
@__java_type_names.287 = internal constant [35 x i8] c"java/lang/reflect/AnnotatedElement\00", align 1
@__java_type_names.288 = internal constant [37 x i8] c"java/lang/reflect/GenericDeclaration\00", align 1
@__java_type_names.289 = internal constant [23 x i8] c"java/lang/reflect/Type\00", align 1
@__java_type_names.290 = internal constant [31 x i8] c"java/lang/reflect/TypeVariable\00", align 1
@__java_type_names.291 = internal constant [32 x i8] c"java/lang/annotation/Annotation\00", align 1
@__java_type_names.292 = internal constant [13 x i8] c"java/io/File\00", align 1
@__java_type_names.293 = internal constant [24 x i8] c"java/io/FileInputStream\00", align 1
@__java_type_names.294 = internal constant [18 x i8] c"java/io/Closeable\00", align 1
@__java_type_names.295 = internal constant [18 x i8] c"java/io/Flushable\00", align 1
@__java_type_names.296 = internal constant [20 x i8] c"java/io/InputStream\00", align 1
@__java_type_names.297 = internal constant [31 x i8] c"java/io/InterruptedIOException\00", align 1
@__java_type_names.298 = internal constant [20 x i8] c"java/io/IOException\00", align 1
@__java_type_names.299 = internal constant [21 x i8] c"java/io/Serializable\00", align 1
@__java_type_names.300 = internal constant [21 x i8] c"java/io/OutputStream\00", align 1
@__java_type_names.301 = internal constant [20 x i8] c"java/io/PrintWriter\00", align 1
@__java_type_names.302 = internal constant [21 x i8] c"java/io/StringWriter\00", align 1
@__java_type_names.303 = internal constant [15 x i8] c"java/io/Writer\00", align 1
@__java_type_names.304 = internal constant [25 x i8] c"mono/android/TypeManager\00", align 1
@__java_type_names.305 = internal constant [35 x i8] c"crc64d4bf71c1e765891b/MainActivity\00", align 1
@__java_type_names.306 = internal constant [55 x i8] c"crc64d4bf71c1e765891b/MainActivity_CustomStringAdapter\00", align 1
@__java_type_names.307 = internal constant [45 x i8] c"crc64d4bf71c1e765891b/MainActivity_TraceView\00", align 1
@__java_type_names.308 = internal constant [39 x i8] c"crc64d4bf71c1e765891b/SyncToPcActivity\00", align 1
@__java_type_names.309 = internal constant [40 x i8] c"crc64d4bf71c1e765891b/TestAndroidLayout\00", align 1
@__java_type_names.310 = internal constant [41 x i8] c"crc64d4bf71c1e765891b/TestRunnerActivity\00", align 1
@__java_type_names.311 = internal constant [37 x i8] c"crc64887e694538fcd9c9/StatsChartView\00", align 1
@__java_type_names.312 = internal constant [61 x i8] c"crc6475b0d5b881bfcfb5/WifiConnector_SpecifierNetworkCallback\00", align 1
@__java_type_names.313 = internal constant [58 x i8] c"crc6475b0d5b881bfcfb5/WifiConnector_LegacyNetworkCallback\00", align 1
@__java_type_names.314 = internal constant [36 x i8] c"crc643c01caa5bd58b444/StatsActivity\00", align 1
@__java_type_names.315 = internal constant [48 x i8] c"androidx/core/view/DragAndDropPermissionsCompat\00", align 1
@__java_type_names.316 = internal constant [36 x i8] c"androidx/core/content/ContextCompat\00", align 1
@__java_type_names.317 = internal constant [35 x i8] c"androidx/core/content/FileProvider\00", align 1
@__java_type_names.318 = internal constant [43 x i8] c"androidx/core/content/pm/PackageInfoCompat\00", align 1
@__java_type_names.319 = internal constant [33 x i8] c"androidx/core/app/ActivityCompat\00", align 1
@__java_type_names.320 = internal constant [58 x i8] c"androidx/core/app/ActivityCompat$PermissionCompatDelegate\00", align 1
@__java_type_names.321 = internal constant [40 x i8] c"androidx/core/app/SharedElementCallback\00", align 1
@__java_type_names.322 = internal constant [70 x i8] c"androidx/core/app/SharedElementCallback$OnSharedElementsReadyListener\00", align 1
@__java_type_names.323 = internal constant [51 x i8] c"com/google/common/util/concurrent/ListenableFuture\00", align 1

@java_type_names = local_unnamed_addr constant [324 x i8*] [
	i8* getelementptr inbounds ([47 x i8], [47 x i8]* @__java_type_names.0, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.1, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.2, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.3, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.4, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.5, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.6, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.7, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.8, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.9, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.10, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.11, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.12, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.13, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.14, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.15, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.16, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.17, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.18, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.19, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.20, i32 0, i32 0),
	i8* getelementptr inbounds ([47 x i8], [47 x i8]* @__java_type_names.21, i32 0, i32 0),
	i8* getelementptr inbounds ([63 x i8], [63 x i8]* @__java_type_names.22, i32 0, i32 0),
	i8* getelementptr inbounds ([50 x i8], [50 x i8]* @__java_type_names.23, i32 0, i32 0),
	i8* getelementptr inbounds ([66 x i8], [66 x i8]* @__java_type_names.24, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.25, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.26, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.27, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.28, i32 0, i32 0),
	i8* getelementptr inbounds ([54 x i8], [54 x i8]* @__java_type_names.29, i32 0, i32 0),
	i8* getelementptr inbounds ([70 x i8], [70 x i8]* @__java_type_names.30, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.31, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.32, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.33, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.34, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.35, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.36, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.37, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.38, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.39, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.40, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.41, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.42, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.43, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.44, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.45, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.46, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.47, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.48, i32 0, i32 0),
	i8* getelementptr inbounds ([50 x i8], [50 x i8]* @__java_type_names.49, i32 0, i32 0),
	i8* getelementptr inbounds ([66 x i8], [66 x i8]* @__java_type_names.50, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.51, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.52, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.53, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.54, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.55, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.56, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.57, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.58, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.59, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.60, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.61, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.62, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.63, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.64, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.65, i32 0, i32 0),
	i8* getelementptr inbounds ([45 x i8], [45 x i8]* @__java_type_names.66, i32 0, i32 0),
	i8* getelementptr inbounds ([46 x i8], [46 x i8]* @__java_type_names.67, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.68, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.69, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.70, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.71, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.72, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.73, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.74, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.75, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.76, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.77, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.78, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.79, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.80, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.81, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.82, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.83, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.84, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.85, i32 0, i32 0),
	i8* getelementptr inbounds ([50 x i8], [50 x i8]* @__java_type_names.86, i32 0, i32 0),
	i8* getelementptr inbounds ([46 x i8], [46 x i8]* @__java_type_names.87, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.88, i32 0, i32 0),
	i8* getelementptr inbounds ([57 x i8], [57 x i8]* @__java_type_names.89, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.90, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.91, i32 0, i32 0),
	i8* getelementptr inbounds ([42 x i8], [42 x i8]* @__java_type_names.92, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.93, i32 0, i32 0),
	i8* getelementptr inbounds ([53 x i8], [53 x i8]* @__java_type_names.94, i32 0, i32 0),
	i8* getelementptr inbounds ([48 x i8], [48 x i8]* @__java_type_names.95, i32 0, i32 0),
	i8* getelementptr inbounds ([56 x i8], [56 x i8]* @__java_type_names.96, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.97, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.98, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.99, i32 0, i32 0),
	i8* getelementptr inbounds ([46 x i8], [46 x i8]* @__java_type_names.100, i32 0, i32 0),
	i8* getelementptr inbounds ([47 x i8], [47 x i8]* @__java_type_names.101, i32 0, i32 0),
	i8* getelementptr inbounds ([52 x i8], [52 x i8]* @__java_type_names.102, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.103, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.104, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.105, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.106, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.107, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.108, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.109, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.110, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.111, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.112, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.113, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.114, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.115, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.116, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.117, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.118, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.119, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.120, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.121, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.122, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.123, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.124, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.125, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.126, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.127, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.128, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.129, i32 0, i32 0),
	i8* getelementptr inbounds ([19 x i8], [19 x i8]* @__java_type_names.130, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.131, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.132, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.133, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.134, i32 0, i32 0),
	i8* getelementptr inbounds ([48 x i8], [48 x i8]* @__java_type_names.135, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.136, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.137, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.138, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.139, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.140, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.141, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.142, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.143, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.144, i32 0, i32 0),
	i8* getelementptr inbounds ([46 x i8], [46 x i8]* @__java_type_names.145, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.146, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.147, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.148, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.149, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.150, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.151, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.152, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.153, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.154, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.155, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.156, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.157, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.158, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.159, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.160, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.161, i32 0, i32 0),
	i8* getelementptr inbounds ([44 x i8], [44 x i8]* @__java_type_names.162, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.163, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.164, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.165, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.166, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.167, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.168, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.169, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.170, i32 0, i32 0),
	i8* getelementptr inbounds ([19 x i8], [19 x i8]* @__java_type_names.171, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.172, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.173, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.174, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.175, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.176, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.177, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.178, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.179, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.180, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.181, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.182, i32 0, i32 0),
	i8* getelementptr inbounds ([48 x i8], [48 x i8]* @__java_type_names.183, i32 0, i32 0),
	i8* getelementptr inbounds ([64 x i8], [64 x i8]* @__java_type_names.184, i32 0, i32 0),
	i8* getelementptr inbounds ([50 x i8], [50 x i8]* @__java_type_names.185, i32 0, i32 0),
	i8* getelementptr inbounds ([66 x i8], [66 x i8]* @__java_type_names.186, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.187, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.188, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.189, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.190, i32 0, i32 0),
	i8* getelementptr inbounds ([67 x i8], [67 x i8]* @__java_type_names.191, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.192, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.193, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.194, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.195, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.196, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.197, i32 0, i32 0),
	i8* getelementptr inbounds ([34 x i8], [34 x i8]* @__java_type_names.198, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.199, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.200, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.201, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.202, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.203, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.204, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.205, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.206, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.207, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.208, i32 0, i32 0),
	i8* getelementptr inbounds ([22 x i8], [22 x i8]* @__java_type_names.209, i32 0, i32 0),
	i8* getelementptr inbounds ([19 x i8], [19 x i8]* @__java_type_names.210, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.211, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.212, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.213, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.214, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.215, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.216, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.217, i32 0, i32 0),
	i8* getelementptr inbounds ([42 x i8], [42 x i8]* @__java_type_names.218, i32 0, i32 0),
	i8* getelementptr inbounds ([43 x i8], [43 x i8]* @__java_type_names.219, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.220, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.221, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.222, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.223, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.224, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.225, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.226, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.227, i32 0, i32 0),
	i8* getelementptr inbounds ([30 x i8], [30 x i8]* @__java_type_names.228, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.229, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.230, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.231, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.232, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.233, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.234, i32 0, i32 0),
	i8* getelementptr inbounds ([38 x i8], [38 x i8]* @__java_type_names.235, i32 0, i32 0),
	i8* getelementptr inbounds ([51 x i8], [51 x i8]* @__java_type_names.236, i32 0, i32 0),
	i8* getelementptr inbounds ([26 x i8], [26 x i8]* @__java_type_names.237, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.238, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.239, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.240, i32 0, i32 0),
	i8* getelementptr inbounds ([15 x i8], [15 x i8]* @__java_type_names.241, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.242, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.243, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.244, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.245, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.246, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.247, i32 0, i32 0),
	i8* getelementptr inbounds ([13 x i8], [13 x i8]* @__java_type_names.248, i32 0, i32 0),
	i8* getelementptr inbounds ([13 x i8], [13 x i8]* @__java_type_names.249, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.250, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.251, i32 0, i32 0),
	i8* getelementptr inbounds ([15 x i8], [15 x i8]* @__java_type_names.252, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.253, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.254, i32 0, i32 0),
	i8* getelementptr inbounds ([29 x i8], [29 x i8]* @__java_type_names.255, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.256, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.257, i32 0, i32 0),
	i8* getelementptr inbounds ([15 x i8], [15 x i8]* @__java_type_names.258, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.259, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.260, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.261, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.262, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.263, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.264, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.265, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.266, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.267, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.268, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.269, i32 0, i32 0),
	i8* getelementptr inbounds ([19 x i8], [19 x i8]* @__java_type_names.270, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.271, i32 0, i32 0),
	i8* getelementptr inbounds ([15 x i8], [15 x i8]* @__java_type_names.272, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.273, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.274, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.275, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.276, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.277, i32 0, i32 0),
	i8* getelementptr inbounds ([27 x i8], [27 x i8]* @__java_type_names.278, i32 0, i32 0),
	i8* getelementptr inbounds ([28 x i8], [28 x i8]* @__java_type_names.279, i32 0, i32 0),
	i8* getelementptr inbounds ([16 x i8], [16 x i8]* @__java_type_names.280, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.281, i32 0, i32 0),
	i8* getelementptr inbounds ([17 x i8], [17 x i8]* @__java_type_names.282, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.283, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.284, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.285, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.286, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.287, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.288, i32 0, i32 0),
	i8* getelementptr inbounds ([23 x i8], [23 x i8]* @__java_type_names.289, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.290, i32 0, i32 0),
	i8* getelementptr inbounds ([32 x i8], [32 x i8]* @__java_type_names.291, i32 0, i32 0),
	i8* getelementptr inbounds ([13 x i8], [13 x i8]* @__java_type_names.292, i32 0, i32 0),
	i8* getelementptr inbounds ([24 x i8], [24 x i8]* @__java_type_names.293, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.294, i32 0, i32 0),
	i8* getelementptr inbounds ([18 x i8], [18 x i8]* @__java_type_names.295, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.296, i32 0, i32 0),
	i8* getelementptr inbounds ([31 x i8], [31 x i8]* @__java_type_names.297, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.298, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.299, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.300, i32 0, i32 0),
	i8* getelementptr inbounds ([20 x i8], [20 x i8]* @__java_type_names.301, i32 0, i32 0),
	i8* getelementptr inbounds ([21 x i8], [21 x i8]* @__java_type_names.302, i32 0, i32 0),
	i8* getelementptr inbounds ([15 x i8], [15 x i8]* @__java_type_names.303, i32 0, i32 0),
	i8* getelementptr inbounds ([25 x i8], [25 x i8]* @__java_type_names.304, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.305, i32 0, i32 0),
	i8* getelementptr inbounds ([55 x i8], [55 x i8]* @__java_type_names.306, i32 0, i32 0),
	i8* getelementptr inbounds ([45 x i8], [45 x i8]* @__java_type_names.307, i32 0, i32 0),
	i8* getelementptr inbounds ([39 x i8], [39 x i8]* @__java_type_names.308, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.309, i32 0, i32 0),
	i8* getelementptr inbounds ([41 x i8], [41 x i8]* @__java_type_names.310, i32 0, i32 0),
	i8* getelementptr inbounds ([37 x i8], [37 x i8]* @__java_type_names.311, i32 0, i32 0),
	i8* getelementptr inbounds ([61 x i8], [61 x i8]* @__java_type_names.312, i32 0, i32 0),
	i8* getelementptr inbounds ([58 x i8], [58 x i8]* @__java_type_names.313, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.314, i32 0, i32 0),
	i8* getelementptr inbounds ([48 x i8], [48 x i8]* @__java_type_names.315, i32 0, i32 0),
	i8* getelementptr inbounds ([36 x i8], [36 x i8]* @__java_type_names.316, i32 0, i32 0),
	i8* getelementptr inbounds ([35 x i8], [35 x i8]* @__java_type_names.317, i32 0, i32 0),
	i8* getelementptr inbounds ([43 x i8], [43 x i8]* @__java_type_names.318, i32 0, i32 0),
	i8* getelementptr inbounds ([33 x i8], [33 x i8]* @__java_type_names.319, i32 0, i32 0),
	i8* getelementptr inbounds ([58 x i8], [58 x i8]* @__java_type_names.320, i32 0, i32 0),
	i8* getelementptr inbounds ([40 x i8], [40 x i8]* @__java_type_names.321, i32 0, i32 0),
	i8* getelementptr inbounds ([70 x i8], [70 x i8]* @__java_type_names.322, i32 0, i32 0),
	i8* getelementptr inbounds ([51 x i8], [51 x i8]* @__java_type_names.323, i32 0, i32 0)
], align 4

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
