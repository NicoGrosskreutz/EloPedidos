	.arch	armv7-a
	.syntax unified
	.eabi_attribute 67, "2.09"	@ Tag_conformance
	.eabi_attribute 6, 10	@ Tag_CPU_arch
	.eabi_attribute 7, 65	@ Tag_CPU_arch_profile
	.eabi_attribute 8, 1	@ Tag_ARM_ISA_use
	.eabi_attribute 9, 2	@ Tag_THUMB_ISA_use
	.fpu	vfpv3-d16
	.eabi_attribute 34, 1	@ Tag_CPU_unaligned_access
	.eabi_attribute 15, 1	@ Tag_ABI_PCS_RW_data
	.eabi_attribute 16, 1	@ Tag_ABI_PCS_RO_data
	.eabi_attribute 17, 2	@ Tag_ABI_PCS_GOT_use
	.eabi_attribute 20, 2	@ Tag_ABI_FP_denormal
	.eabi_attribute 21, 0	@ Tag_ABI_FP_exceptions
	.eabi_attribute 23, 3	@ Tag_ABI_FP_number_model
	.eabi_attribute 24, 1	@ Tag_ABI_align_needed
	.eabi_attribute 25, 1	@ Tag_ABI_align_preserved
	.eabi_attribute 38, 1	@ Tag_ABI_FP_16bit_format
	.eabi_attribute 18, 4	@ Tag_ABI_PCS_wchar_t
	.eabi_attribute 26, 2	@ Tag_ABI_enum_size
	.eabi_attribute 14, 0	@ Tag_ABI_PCS_R9_use
	.file	"typemaps.armeabi-v7a.s"

/* map_module_count: START */
	.section	.rodata.map_module_count,"a",%progbits
	.type	map_module_count, %object
	.p2align	2
	.global	map_module_count
map_module_count:
	.size	map_module_count, 4
	.long	21
/* map_module_count: END */

/* java_type_count: START */
	.section	.rodata.java_type_count,"a",%progbits
	.type	java_type_count, %object
	.p2align	2
	.global	java_type_count
java_type_count:
	.size	java_type_count, 4
	.long	607
/* java_type_count: END */

	.include	"typemaps.armeabi-v7a-shared.inc"
	.include	"typemaps.armeabi-v7a-managed.inc"

/* Managed to Java map: START */
	.section	.data.rel.map_modules,"aw",%progbits
	.type	map_modules, %object
	.p2align	2
	.global	map_modules
map_modules:
	/* module_uuid: 2d694413-0fa4-4097-8467-dd0590e155e5 */
	.byte	0x13, 0x44, 0x69, 0x2d, 0xa4, 0x0f, 0x97, 0x40, 0x84, 0x67, 0xdd, 0x05, 0x90, 0xe1, 0x55, 0xe5
	/* entry_count */
	.long	4
	/* duplicate_count */
	.long	1
	/* map */
	.long	module0_managed_to_java
	/* duplicate_map */
	.long	module0_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.Common */
	.long	.L.map_aname.0
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 63580a17-2d08-49b9-a0ca-00e61312cd54 */
	.byte	0x17, 0x0a, 0x58, 0x63, 0x08, 0x2d, 0xb9, 0x49, 0xa0, 0xca, 0x00, 0xe6, 0x13, 0x12, 0xcd, 0x54
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module1_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Plugin.CurrentActivity */
	.long	.L.map_aname.1
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 2240341f-4216-4adf-9675-613a7d38e6a2 */
	.byte	0x1f, 0x34, 0x40, 0x22, 0x16, 0x42, 0xdf, 0x4a, 0x96, 0x75, 0x61, 0x3a, 0x7d, 0x38, 0xe6, 0xa2
	/* entry_count */
	.long	10
	/* duplicate_count */
	.long	0
	/* map */
	.long	module2_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: SkiaSharp.Views.Android */
	.long	.L.map_aname.2
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 15d0f12b-6ebc-43db-8504-fc6197b4762b */
	.byte	0x2b, 0xf1, 0xd0, 0x15, 0xbc, 0x6e, 0xdb, 0x43, 0x85, 0x04, 0xfc, 0x61, 0x97, 0xb4, 0x76, 0x2b
	/* entry_count */
	.long	44
	/* duplicate_count */
	.long	0
	/* map */
	.long	module3_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: EloPedidos */
	.long	.L.map_aname.3
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 5b258832-6da0-4300-8268-e4a71ab70d2e */
	.byte	0x32, 0x88, 0x25, 0x5b, 0xa0, 0x6d, 0x00, 0x43, 0x82, 0x68, 0xe4, 0xa7, 0x1a, 0xb7, 0x0d, 0x2e
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module4_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.v7.CardView */
	.long	.L.map_aname.4
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: e9965338-b9dc-4721-ac65-85abd89cdd9f */
	.byte	0x38, 0x53, 0x96, 0xe9, 0xdc, 0xb9, 0x21, 0x47, 0xac, 0x65, 0x85, 0xab, 0xd8, 0x9c, 0xdd, 0x9f
	/* entry_count */
	.long	395
	/* duplicate_count */
	.long	188
	/* map */
	.long	module5_managed_to_java
	/* duplicate_map */
	.long	module5_managed_to_java_duplicates
	/* assembly_name: Mono.Android */
	.long	.L.map_aname.5
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: f55e6e3a-690d-4ac6-ba0f-5b60471ad9a7 */
	.byte	0x3a, 0x6e, 0x5e, 0xf5, 0x0d, 0x69, 0xc6, 0x4a, 0xba, 0x0f, 0x5b, 0x60, 0x47, 0x1a, 0xd9, 0xa7
	/* entry_count */
	.long	37
	/* duplicate_count */
	.long	3
	/* map */
	.long	module6_managed_to_java
	/* duplicate_map */
	.long	module6_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Compat */
	.long	.L.map_aname.6
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: e6cb6943-6f5b-455f-a0a6-6dc46ca4b792 */
	.byte	0x43, 0x69, 0xcb, 0xe6, 0x5b, 0x6f, 0x5f, 0x45, 0xa0, 0xa6, 0x6d, 0xc4, 0x6c, 0xa4, 0xb7, 0x92
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module7_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Plugin.Geolocator */
	.long	.L.map_aname.7
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 47def747-0984-49e8-a51f-e98384a2ccf7 */
	.byte	0x47, 0xf7, 0xde, 0x47, 0x84, 0x09, 0xe8, 0x49, 0xa5, 0x1f, 0xe9, 0x83, 0x84, 0xa2, 0xcc, 0xf7
	/* entry_count */
	.long	5
	/* duplicate_count */
	.long	1
	/* map */
	.long	module8_managed_to_java
	/* duplicate_map */
	.long	module8_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Loader */
	.long	.L.map_aname.8
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 3052f548-e96f-4679-a533-914e324ed9a1 */
	.byte	0x48, 0xf5, 0x52, 0x30, 0x6f, 0xe9, 0x79, 0x46, 0xa5, 0x33, 0x91, 0x4e, 0x32, 0x4e, 0xd9, 0xa1
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	1
	/* map */
	.long	module9_managed_to_java
	/* duplicate_map */
	.long	module9_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.LiveData.Core */
	.long	.L.map_aname.9
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: a9ff174f-27b6-42b3-bcaa-a3b93553c96c */
	.byte	0x4f, 0x17, 0xff, 0xa9, 0xb6, 0x27, 0xb3, 0x42, 0xbc, 0xaa, 0xa3, 0xb9, 0x35, 0x53, 0xc9, 0x6c
	/* entry_count */
	.long	4
	/* duplicate_count */
	.long	0
	/* map */
	.long	module10_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: ZXing.Net.Mobile */
	.long	.L.map_aname.10
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 69a8a04f-c61e-464d-ab55-1bf20572b964 */
	.byte	0x4f, 0xa0, 0xa8, 0x69, 0x1e, 0xc6, 0x4d, 0x46, 0xab, 0x55, 0x1b, 0xf2, 0x05, 0x72, 0xb9, 0x64
	/* entry_count */
	.long	18
	/* duplicate_count */
	.long	3
	/* map */
	.long	module11_managed_to_java
	/* duplicate_map */
	.long	module11_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Design */
	.long	.L.map_aname.11
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 9f8bad59-ca13-4852-b4b0-1016d5595293 */
	.byte	0x59, 0xad, 0x8b, 0x9f, 0x13, 0xca, 0x52, 0x48, 0xb4, 0xb0, 0x10, 0x16, 0xd5, 0x59, 0x52, 0x93
	/* entry_count */
	.long	25
	/* duplicate_count */
	.long	8
	/* map */
	.long	module12_managed_to_java
	/* duplicate_map */
	.long	module12_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Arch.Work.Runtime */
	.long	.L.map_aname.12
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 4a59d066-affb-4842-838c-29cfe8e9436a */
	.byte	0x66, 0xd0, 0x59, 0x4a, 0xfb, 0xaf, 0x42, 0x48, 0x83, 0x8c, 0x29, 0xcf, 0xe8, 0xe9, 0x43, 0x6a
	/* entry_count */
	.long	3
	/* duplicate_count */
	.long	1
	/* map */
	.long	module13_managed_to_java
	/* duplicate_map */
	.long	module13_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.CoordinaterLayout */
	.long	.L.map_aname.13
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 953f5d72-14d5-47e2-a0ae-71f91bbdb5e1 */
	.byte	0x72, 0x5d, 0x3f, 0x95, 0xd5, 0x14, 0xe2, 0x47, 0xa0, 0xae, 0x71, 0xf9, 0x1b, 0xbd, 0xb5, 0xe1
	/* entry_count */
	.long	9
	/* duplicate_count */
	.long	3
	/* map */
	.long	module14_managed_to_java
	/* duplicate_map */
	.long	module14_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Fragment */
	.long	.L.map_aname.14
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 074d7973-5add-43c9-83ef-ce04655f81b3 */
	.byte	0x73, 0x79, 0x4d, 0x07, 0xdd, 0x5a, 0xc9, 0x43, 0x83, 0xef, 0xce, 0x04, 0x65, 0x5f, 0x81, 0xb3
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module15_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Microcharts.Droid */
	.long	.L.map_aname.15
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: a43d4d82-8699-4869-97b2-8a3979c306ee */
	.byte	0x82, 0x4d, 0x3d, 0xa4, 0x99, 0x86, 0x69, 0x48, 0x97, 0xb2, 0x8a, 0x39, 0x79, 0xc3, 0x06, 0xee
	/* entry_count */
	.long	38
	/* duplicate_count */
	.long	4
	/* map */
	.long	module16_managed_to_java
	/* duplicate_map */
	.long	module16_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.v7.AppCompat */
	.long	.L.map_aname.16
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 6a233691-915d-4c0b-a001-09258edbaf8e */
	.byte	0x91, 0x36, 0x23, 0x6a, 0x5d, 0x91, 0x0b, 0x4c, 0xa0, 0x01, 0x09, 0x25, 0x8e, 0xdb, 0xaf, 0x8e
	/* entry_count */
	.long	3
	/* duplicate_count */
	.long	0
	/* map */
	.long	module17_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.DrawerLayout */
	.long	.L.map_aname.17
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: f23d40d0-e96f-4f05-b366-721b578c9d9f */
	.byte	0xd0, 0x40, 0x3d, 0xf2, 0x6f, 0xe9, 0x05, 0x4f, 0xb3, 0x66, 0x72, 0x1b, 0x57, 0x8c, 0x9d, 0x9f
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module18_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.ViewModel */
	.long	.L.map_aname.18
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 5fc473ee-01b2-462c-b092-73117439f998 */
	.byte	0xee, 0x73, 0xc4, 0x5f, 0xb2, 0x01, 0x2c, 0x46, 0xb0, 0x92, 0x73, 0x11, 0x74, 0x39, 0xf9, 0x98
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module19_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Mapsui.UI.Android */
	.long	.L.map_aname.19
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 358110ff-2278-4838-bcf2-aefb1caf26ff */
	.byte	0xff, 0x10, 0x81, 0x35, 0x78, 0x22, 0x38, 0x48, 0xbc, 0xf2, 0xae, 0xfb, 0x1c, 0xaf, 0x26, 0xff
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module20_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Essentials */
	.long	.L.map_aname.20
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	.size	map_modules, 1008
/* Managed to Java map: END */

/* Java to managed map: START */
	.section	.rodata.map_java,"a",%progbits
	.type	map_java, %object
	.p2align	2
	.global	map_java
map_java:
	/* #0 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554537
	/* java_name */
	.ascii	"Broadcast/cancelNotification"
	.zero	61
	.zero	1

	/* #1 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554469
	/* java_name */
	.ascii	"Services/GeolocatorService"
	.zero	63
	.zero	1

	/* #2 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"Services/LocationService"
	.zero	65
	.zero	1

	/* #3 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555016
	/* java_name */
	.ascii	"android/animation/Animator"
	.zero	63
	.zero	1

	/* #4 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/animation/Animator$AnimatorListener"
	.zero	46
	.zero	1

	/* #5 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/animation/Animator$AnimatorPauseListener"
	.zero	41
	.zero	1

	/* #6 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555022
	/* java_name */
	.ascii	"android/animation/AnimatorListenerAdapter"
	.zero	48
	.zero	1

	/* #7 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/animation/TimeInterpolator"
	.zero	55
	.zero	1

	/* #8 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555028
	/* java_name */
	.ascii	"android/app/Activity"
	.zero	69
	.zero	1

	/* #9 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555029
	/* java_name */
	.ascii	"android/app/ActivityManager"
	.zero	62
	.zero	1

	/* #10 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555030
	/* java_name */
	.ascii	"android/app/ActivityManager$RunningServiceInfo"
	.zero	43
	.zero	1

	/* #11 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555031
	/* java_name */
	.ascii	"android/app/AlertDialog"
	.zero	66
	.zero	1

	/* #12 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555032
	/* java_name */
	.ascii	"android/app/AlertDialog$Builder"
	.zero	58
	.zero	1

	/* #13 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555033
	/* java_name */
	.ascii	"android/app/Application"
	.zero	66
	.zero	1

	/* #14 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/app/Application$ActivityLifecycleCallbacks"
	.zero	39
	.zero	1

	/* #15 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555036
	/* java_name */
	.ascii	"android/app/DatePickerDialog"
	.zero	61
	.zero	1

	/* #16 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/app/DatePickerDialog$OnDateSetListener"
	.zero	43
	.zero	1

	/* #17 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555041
	/* java_name */
	.ascii	"android/app/Dialog"
	.zero	71
	.zero	1

	/* #18 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555043
	/* java_name */
	.ascii	"android/app/Notification"
	.zero	65
	.zero	1

	/* #19 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555044
	/* java_name */
	.ascii	"android/app/Notification$Builder"
	.zero	57
	.zero	1

	/* #20 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555052
	/* java_name */
	.ascii	"android/app/NotificationChannel"
	.zero	58
	.zero	1

	/* #21 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555045
	/* java_name */
	.ascii	"android/app/NotificationManager"
	.zero	58
	.zero	1

	/* #22 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555055
	/* java_name */
	.ascii	"android/app/PendingIntent"
	.zero	64
	.zero	1

	/* #23 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555058
	/* java_name */
	.ascii	"android/app/Service"
	.zero	70
	.zero	1

	/* #24 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555049
	/* java_name */
	.ascii	"android/app/UiModeManager"
	.zero	64
	.zero	1

	/* #25 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/arch/lifecycle/Lifecycle"
	.zero	57
	.zero	1

	/* #26 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/arch/lifecycle/Lifecycle$State"
	.zero	51
	.zero	1

	/* #27 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/arch/lifecycle/LifecycleObserver"
	.zero	49
	.zero	1

	/* #28 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/arch/lifecycle/LifecycleOwner"
	.zero	52
	.zero	1

	/* #29 */
	/* module_index */
	.long	9
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/arch/lifecycle/LiveData"
	.zero	58
	.zero	1

	/* #30 */
	/* module_index */
	.long	9
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/arch/lifecycle/Observer"
	.zero	58
	.zero	1

	/* #31 */
	/* module_index */
	.long	18
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/arch/lifecycle/ViewModelStore"
	.zero	52
	.zero	1

	/* #32 */
	/* module_index */
	.long	18
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/arch/lifecycle/ViewModelStoreOwner"
	.zero	47
	.zero	1

	/* #33 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555014
	/* java_name */
	.ascii	"android/bluetooth/BluetoothAdapter"
	.zero	55
	.zero	1

	/* #34 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555013
	/* java_name */
	.ascii	"android/bluetooth/BluetoothDevice"
	.zero	56
	.zero	1

	/* #35 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555015
	/* java_name */
	.ascii	"android/bluetooth/BluetoothSocket"
	.zero	56
	.zero	1

	/* #36 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555068
	/* java_name */
	.ascii	"android/content/BroadcastReceiver"
	.zero	56
	.zero	1

	/* #37 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/ComponentCallbacks"
	.zero	55
	.zero	1

	/* #38 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/ComponentCallbacks2"
	.zero	54
	.zero	1

	/* #39 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555070
	/* java_name */
	.ascii	"android/content/ComponentName"
	.zero	60
	.zero	1

	/* #40 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555063
	/* java_name */
	.ascii	"android/content/ContentProvider"
	.zero	58
	.zero	1

	/* #41 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555072
	/* java_name */
	.ascii	"android/content/ContentResolver"
	.zero	58
	.zero	1

	/* #42 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555064
	/* java_name */
	.ascii	"android/content/ContentValues"
	.zero	60
	.zero	1

	/* #43 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555065
	/* java_name */
	.ascii	"android/content/Context"
	.zero	66
	.zero	1

	/* #44 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555075
	/* java_name */
	.ascii	"android/content/ContextWrapper"
	.zero	59
	.zero	1

	/* #45 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface"
	.zero	58
	.zero	1

	/* #46 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface$OnCancelListener"
	.zero	41
	.zero	1

	/* #47 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface$OnClickListener"
	.zero	42
	.zero	1

	/* #48 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface$OnDismissListener"
	.zero	40
	.zero	1

	/* #49 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface$OnKeyListener"
	.zero	44
	.zero	1

	/* #50 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/DialogInterface$OnMultiChoiceClickListener"
	.zero	31
	.zero	1

	/* #51 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555066
	/* java_name */
	.ascii	"android/content/Intent"
	.zero	67
	.zero	1

	/* #52 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555096
	/* java_name */
	.ascii	"android/content/IntentSender"
	.zero	61
	.zero	1

	/* #53 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/SharedPreferences"
	.zero	56
	.zero	1

	/* #54 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/SharedPreferences$Editor"
	.zero	49
	.zero	1

	/* #55 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/content/SharedPreferences$OnSharedPreferenceChangeListener"
	.zero	23
	.zero	1

	/* #56 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555104
	/* java_name */
	.ascii	"android/content/pm/ApplicationInfo"
	.zero	55
	.zero	1

	/* #57 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555106
	/* java_name */
	.ascii	"android/content/pm/ConfigurationInfo"
	.zero	53
	.zero	1

	/* #58 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555107
	/* java_name */
	.ascii	"android/content/pm/PackageInfo"
	.zero	59
	.zero	1

	/* #59 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555109
	/* java_name */
	.ascii	"android/content/pm/PackageItemInfo"
	.zero	55
	.zero	1

	/* #60 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555110
	/* java_name */
	.ascii	"android/content/pm/PackageManager"
	.zero	56
	.zero	1

	/* #61 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555113
	/* java_name */
	.ascii	"android/content/pm/PermissionInfo"
	.zero	56
	.zero	1

	/* #62 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555115
	/* java_name */
	.ascii	"android/content/res/ColorStateList"
	.zero	55
	.zero	1

	/* #63 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555116
	/* java_name */
	.ascii	"android/content/res/Configuration"
	.zero	56
	.zero	1

	/* #64 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555117
	/* java_name */
	.ascii	"android/content/res/Resources"
	.zero	60
	.zero	1

	/* #65 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555118
	/* java_name */
	.ascii	"android/content/res/Resources$Theme"
	.zero	54
	.zero	1

	/* #66 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555119
	/* java_name */
	.ascii	"android/content/res/TypedArray"
	.zero	59
	.zero	1

	/* #67 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554648
	/* java_name */
	.ascii	"android/database/CharArrayBuffer"
	.zero	57
	.zero	1

	/* #68 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554649
	/* java_name */
	.ascii	"android/database/ContentObserver"
	.zero	57
	.zero	1

	/* #69 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/database/Cursor"
	.zero	66
	.zero	1

	/* #70 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554651
	/* java_name */
	.ascii	"android/database/DataSetObserver"
	.zero	57
	.zero	1

	/* #71 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554987
	/* java_name */
	.ascii	"android/graphics/Bitmap"
	.zero	66
	.zero	1

	/* #72 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554989
	/* java_name */
	.ascii	"android/graphics/Bitmap$Config"
	.zero	59
	.zero	1

	/* #73 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554990
	/* java_name */
	.ascii	"android/graphics/Canvas"
	.zero	66
	.zero	1

	/* #74 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554994
	/* java_name */
	.ascii	"android/graphics/Color"
	.zero	67
	.zero	1

	/* #75 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554993
	/* java_name */
	.ascii	"android/graphics/ColorFilter"
	.zero	61
	.zero	1

	/* #76 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554997
	/* java_name */
	.ascii	"android/graphics/Matrix"
	.zero	66
	.zero	1

	/* #77 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554998
	/* java_name */
	.ascii	"android/graphics/Paint"
	.zero	67
	.zero	1

	/* #78 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554999
	/* java_name */
	.ascii	"android/graphics/Paint$Style"
	.zero	61
	.zero	1

	/* #79 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555001
	/* java_name */
	.ascii	"android/graphics/Point"
	.zero	67
	.zero	1

	/* #80 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555002
	/* java_name */
	.ascii	"android/graphics/PointF"
	.zero	66
	.zero	1

	/* #81 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555003
	/* java_name */
	.ascii	"android/graphics/PorterDuff"
	.zero	62
	.zero	1

	/* #82 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555004
	/* java_name */
	.ascii	"android/graphics/PorterDuff$Mode"
	.zero	57
	.zero	1

	/* #83 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555005
	/* java_name */
	.ascii	"android/graphics/Rect"
	.zero	68
	.zero	1

	/* #84 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555006
	/* java_name */
	.ascii	"android/graphics/RectF"
	.zero	67
	.zero	1

	/* #85 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555007
	/* java_name */
	.ascii	"android/graphics/SurfaceTexture"
	.zero	58
	.zero	1

	/* #86 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555008
	/* java_name */
	.ascii	"android/graphics/Typeface"
	.zero	64
	.zero	1

	/* #87 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555009
	/* java_name */
	.ascii	"android/graphics/drawable/Drawable"
	.zero	55
	.zero	1

	/* #88 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/graphics/drawable/Drawable$Callback"
	.zero	46
	.zero	1

	/* #89 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554977
	/* java_name */
	.ascii	"android/hardware/Camera"
	.zero	66
	.zero	1

	/* #90 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/hardware/Camera$AutoFocusCallback"
	.zero	48
	.zero	1

	/* #91 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554980
	/* java_name */
	.ascii	"android/hardware/Camera$CameraInfo"
	.zero	55
	.zero	1

	/* #92 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554981
	/* java_name */
	.ascii	"android/hardware/Camera$Parameters"
	.zero	55
	.zero	1

	/* #93 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/hardware/Camera$PreviewCallback"
	.zero	50
	.zero	1

	/* #94 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554984
	/* java_name */
	.ascii	"android/hardware/Camera$Size"
	.zero	61
	.zero	1

	/* #95 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554966
	/* java_name */
	.ascii	"android/location/Address"
	.zero	65
	.zero	1

	/* #96 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554968
	/* java_name */
	.ascii	"android/location/Criteria"
	.zero	64
	.zero	1

	/* #97 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554969
	/* java_name */
	.ascii	"android/location/Geocoder"
	.zero	64
	.zero	1

	/* #98 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554974
	/* java_name */
	.ascii	"android/location/Location"
	.zero	64
	.zero	1

	/* #99 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/location/LocationListener"
	.zero	56
	.zero	1

	/* #100 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554964
	/* java_name */
	.ascii	"android/location/LocationManager"
	.zero	57
	.zero	1

	/* #101 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554975
	/* java_name */
	.ascii	"android/location/LocationProvider"
	.zero	56
	.zero	1

	/* #102 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554959
	/* java_name */
	.ascii	"android/media/MediaPlayer"
	.zero	64
	.zero	1

	/* #103 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/media/VolumeAutomation"
	.zero	59
	.zero	1

	/* #104 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554962
	/* java_name */
	.ascii	"android/media/VolumeShaper"
	.zero	63
	.zero	1

	/* #105 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554963
	/* java_name */
	.ascii	"android/media/VolumeShaper$Configuration"
	.zero	49
	.zero	1

	/* #106 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554954
	/* java_name */
	.ascii	"android/net/ConnectivityManager"
	.zero	58
	.zero	1

	/* #107 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554955
	/* java_name */
	.ascii	"android/net/Network"
	.zero	70
	.zero	1

	/* #108 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554956
	/* java_name */
	.ascii	"android/net/NetworkInfo"
	.zero	66
	.zero	1

	/* #109 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554957
	/* java_name */
	.ascii	"android/net/Uri"
	.zero	74
	.zero	1

	/* #110 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554927
	/* java_name */
	.ascii	"android/opengl/GLDebugHelper"
	.zero	61
	.zero	1

	/* #111 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554928
	/* java_name */
	.ascii	"android/opengl/GLES10"
	.zero	68
	.zero	1

	/* #112 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554929
	/* java_name */
	.ascii	"android/opengl/GLES20"
	.zero	68
	.zero	1

	/* #113 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554923
	/* java_name */
	.ascii	"android/opengl/GLSurfaceView"
	.zero	61
	.zero	1

	/* #114 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/opengl/GLSurfaceView$Renderer"
	.zero	52
	.zero	1

	/* #115 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554933
	/* java_name */
	.ascii	"android/os/BaseBundle"
	.zero	68
	.zero	1

	/* #116 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554934
	/* java_name */
	.ascii	"android/os/Build"
	.zero	73
	.zero	1

	/* #117 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554935
	/* java_name */
	.ascii	"android/os/Build$VERSION"
	.zero	65
	.zero	1

	/* #118 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554937
	/* java_name */
	.ascii	"android/os/Bundle"
	.zero	72
	.zero	1

	/* #119 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554938
	/* java_name */
	.ascii	"android/os/Environment"
	.zero	67
	.zero	1

	/* #120 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554931
	/* java_name */
	.ascii	"android/os/Handler"
	.zero	71
	.zero	1

	/* #121 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554939
	/* java_name */
	.ascii	"android/os/HandlerThread"
	.zero	65
	.zero	1

	/* #122 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/os/IBinder"
	.zero	71
	.zero	1

	/* #123 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/os/IBinder$DeathRecipient"
	.zero	56
	.zero	1

	/* #124 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/os/IInterface"
	.zero	68
	.zero	1

	/* #125 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554950
	/* java_name */
	.ascii	"android/os/Looper"
	.zero	72
	.zero	1

	/* #126 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554932
	/* java_name */
	.ascii	"android/os/Message"
	.zero	71
	.zero	1

	/* #127 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554951
	/* java_name */
	.ascii	"android/os/Parcel"
	.zero	72
	.zero	1

	/* #128 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/os/Parcelable"
	.zero	68
	.zero	1

	/* #129 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/os/Parcelable$Creator"
	.zero	60
	.zero	1

	/* #130 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554922
	/* java_name */
	.ascii	"android/preference/PreferenceManager"
	.zero	53
	.zero	1

	/* #131 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554644
	/* java_name */
	.ascii	"android/provider/Settings"
	.zero	64
	.zero	1

	/* #132 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554645
	/* java_name */
	.ascii	"android/provider/Settings$NameValueTable"
	.zero	49
	.zero	1

	/* #133 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554646
	/* java_name */
	.ascii	"android/provider/Settings$Secure"
	.zero	57
	.zero	1

	/* #134 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554647
	/* java_name */
	.ascii	"android/provider/Settings$System"
	.zero	57
	.zero	1

	/* #135 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555166
	/* java_name */
	.ascii	"android/runtime/JavaProxyThrowable"
	.zero	55
	.zero	1

	/* #136 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/design/animation/MotionSpec"
	.zero	46
	.zero	1

	/* #137 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/design/animation/MotionTiming"
	.zero	44
	.zero	1

	/* #138 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/design/expandable/ExpandableTransformationWidget"
	.zero	25
	.zero	1

	/* #139 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/design/expandable/ExpandableWidget"
	.zero	39
	.zero	1

	/* #140 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/design/snackbar/ContentViewCallback"
	.zero	38
	.zero	1

	/* #141 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"android/support/design/widget/BaseTransientBottomBar"
	.zero	37
	.zero	1

	/* #142 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"android/support/design/widget/BaseTransientBottomBar$BaseCallback"
	.zero	24
	.zero	1

	/* #143 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"android/support/design/widget/BaseTransientBottomBar$Behavior"
	.zero	28
	.zero	1

	/* #144 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout"
	.zero	42
	.zero	1

	/* #145 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout$Behavior"
	.zero	33
	.zero	1

	/* #146 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout$LayoutParams"
	.zero	29
	.zero	1

	/* #147 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/design/widget/FloatingActionButton"
	.zero	39
	.zero	1

	/* #148 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"android/support/design/widget/FloatingActionButton$OnVisibilityChangedListener"
	.zero	11
	.zero	1

	/* #149 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"android/support/design/widget/Snackbar"
	.zero	51
	.zero	1

	/* #150 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"android/support/design/widget/Snackbar$Callback"
	.zero	42
	.zero	1

	/* #151 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/design/widget/Snackbar_SnackbarActionClickImplementor"
	.zero	20
	.zero	1

	/* #152 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"android/support/design/widget/SwipeDismissBehavior"
	.zero	39
	.zero	1

	/* #153 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"android/support/design/widget/SwipeDismissBehavior$OnDismissListener"
	.zero	21
	.zero	1

	/* #154 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"android/support/design/widget/TextInputEditText"
	.zero	42
	.zero	1

	/* #155 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"android/support/design/widget/VisibilityAwareImageButton"
	.zero	33
	.zero	1

	/* #156 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v13/view/DragAndDropPermissionsCompat"
	.zero	36
	.zero	1

	/* #157 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat"
	.zero	52
	.zero	1

	/* #158 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$OnRequestPermissionsResultCallback"
	.zero	17
	.zero	1

	/* #159 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$PermissionCompatDelegate"
	.zero	27
	.zero	1

	/* #160 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$RequestPermissionsRequestCodeValidator"
	.zero	13
	.zero	1

	/* #161 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/app/Fragment"
	.zero	58
	.zero	1

	/* #162 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/app/Fragment$SavedState"
	.zero	47
	.zero	1

	/* #163 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v4/app/FragmentActivity"
	.zero	50
	.zero	1

	/* #164 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager"
	.zero	51
	.zero	1

	/* #165 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$BackStackEntry"
	.zero	36
	.zero	1

	/* #166 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$FragmentLifecycleCallbacks"
	.zero	24
	.zero	1

	/* #167 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$OnBackStackChangedListener"
	.zero	24
	.zero	1

	/* #168 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"android/support/v4/app/FragmentTransaction"
	.zero	47
	.zero	1

	/* #169 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/app/LoaderManager"
	.zero	53
	.zero	1

	/* #170 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/v4/app/LoaderManager$LoaderCallbacks"
	.zero	37
	.zero	1

	/* #171 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554480
	/* java_name */
	.ascii	"android/support/v4/app/NotificationBuilderWithBuilderAccessor"
	.zero	28
	.zero	1

	/* #172 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"android/support/v4/app/NotificationCompat"
	.zero	48
	.zero	1

	/* #173 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"android/support/v4/app/NotificationCompat$Action"
	.zero	41
	.zero	1

	/* #174 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554483
	/* java_name */
	.ascii	"android/support/v4/app/NotificationCompat$Builder"
	.zero	40
	.zero	1

	/* #175 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"android/support/v4/app/NotificationCompat$Extender"
	.zero	39
	.zero	1

	/* #176 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554486
	/* java_name */
	.ascii	"android/support/v4/app/NotificationCompat$Style"
	.zero	42
	.zero	1

	/* #177 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554488
	/* java_name */
	.ascii	"android/support/v4/app/NotificationManagerCompat"
	.zero	41
	.zero	1

	/* #178 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554489
	/* java_name */
	.ascii	"android/support/v4/app/RemoteInput"
	.zero	55
	.zero	1

	/* #179 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"android/support/v4/app/SharedElementCallback"
	.zero	45
	.zero	1

	/* #180 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"android/support/v4/app/SharedElementCallback$OnSharedElementsReadyListener"
	.zero	15
	.zero	1

	/* #181 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554494
	/* java_name */
	.ascii	"android/support/v4/app/TaskStackBuilder"
	.zero	50
	.zero	1

	/* #182 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554496
	/* java_name */
	.ascii	"android/support/v4/app/TaskStackBuilder$SupportParentable"
	.zero	32
	.zero	1

	/* #183 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554468
	/* java_name */
	.ascii	"android/support/v4/content/ContextCompat"
	.zero	49
	.zero	1

	/* #184 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554469
	/* java_name */
	.ascii	"android/support/v4/content/FileProvider"
	.zero	50
	.zero	1

	/* #185 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v4/content/Loader"
	.zero	56
	.zero	1

	/* #186 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/content/Loader$OnLoadCanceledListener"
	.zero	33
	.zero	1

	/* #187 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/v4/content/Loader$OnLoadCompleteListener"
	.zero	33
	.zero	1

	/* #188 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"android/support/v4/content/PermissionChecker"
	.zero	45
	.zero	1

	/* #189 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"android/support/v4/content/pm/PackageInfoCompat"
	.zero	42
	.zero	1

	/* #190 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"android/support/v4/internal/view/SupportMenu"
	.zero	45
	.zero	1

	/* #191 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"android/support/v4/internal/view/SupportMenuItem"
	.zero	41
	.zero	1

	/* #192 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider"
	.zero	51
	.zero	1

	/* #193 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider$SubUiVisibilityListener"
	.zero	27
	.zero	1

	/* #194 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider$VisibilityListener"
	.zero	32
	.zero	1

	/* #195 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/v4/view/DisplayCutoutCompat"
	.zero	46
	.zero	1

	/* #196 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingParent"
	.zero	44
	.zero	1

	/* #197 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingParent2"
	.zero	43
	.zero	1

	/* #198 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"android/support/v4/view/TintableBackgroundView"
	.zero	43
	.zero	1

	/* #199 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorCompat"
	.zero	39
	.zero	1

	/* #200 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorListener"
	.zero	37
	.zero	1

	/* #201 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorUpdateListener"
	.zero	31
	.zero	1

	/* #202 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"android/support/v4/view/WindowInsetsCompat"
	.zero	47
	.zero	1

	/* #203 */
	/* module_index */
	.long	17
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v4/widget/DrawerLayout"
	.zero	51
	.zero	1

	/* #204 */
	/* module_index */
	.long	17
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/widget/DrawerLayout$DrawerListener"
	.zero	36
	.zero	1

	/* #205 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/widget/TintableImageSourceView"
	.zero	40
	.zero	1

	/* #206 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar"
	.zero	57
	.zero	1

	/* #207 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$LayoutParams"
	.zero	44
	.zero	1

	/* #208 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$OnMenuVisibilityListener"
	.zero	32
	.zero	1

	/* #209 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$OnNavigationListener"
	.zero	36
	.zero	1

	/* #210 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$Tab"
	.zero	53
	.zero	1

	/* #211 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$TabListener"
	.zero	45
	.zero	1

	/* #212 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle"
	.zero	45
	.zero	1

	/* #213 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle$Delegate"
	.zero	36
	.zero	1

	/* #214 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle$DelegateProvider"
	.zero	28
	.zero	1

	/* #215 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v7/app/AlertDialog"
	.zero	55
	.zero	1

	/* #216 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v7/app/AlertDialog$Builder"
	.zero	47
	.zero	1

	/* #217 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/v7/app/AlertDialog_IDialogInterfaceOnCancelListenerImplementor"
	.zero	11
	.zero	1

	/* #218 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v7/app/AlertDialog_IDialogInterfaceOnClickListenerImplementor"
	.zero	12
	.zero	1

	/* #219 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v7/app/AlertDialog_IDialogInterfaceOnMultiChoiceClickListenerImplementor"
	.zero	1
	.zero	1

	/* #220 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatActivity"
	.zero	49
	.zero	1

	/* #221 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatCallback"
	.zero	49
	.zero	1

	/* #222 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatDelegate"
	.zero	49
	.zero	1

	/* #223 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatDialog"
	.zero	51
	.zero	1

	/* #224 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v7/graphics/drawable/DrawerArrowDrawable"
	.zero	33
	.zero	1

	/* #225 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"android/support/v7/view/ActionMode"
	.zero	55
	.zero	1

	/* #226 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554483
	/* java_name */
	.ascii	"android/support/v7/view/ActionMode$Callback"
	.zero	46
	.zero	1

	/* #227 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuBuilder"
	.zero	49
	.zero	1

	/* #228 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554487
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuBuilder$Callback"
	.zero	40
	.zero	1

	/* #229 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554494
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuItemImpl"
	.zero	48
	.zero	1

	/* #230 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554491
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuPresenter"
	.zero	47
	.zero	1

	/* #231 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554489
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuPresenter$Callback"
	.zero	38
	.zero	1

	/* #232 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuView"
	.zero	52
	.zero	1

	/* #233 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554495
	/* java_name */
	.ascii	"android/support/v7/view/menu/SubMenuBuilder"
	.zero	46
	.zero	1

	/* #234 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"android/support/v7/widget/AppCompatEditText"
	.zero	46
	.zero	1

	/* #235 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"android/support/v7/widget/CardView"
	.zero	55
	.zero	1

	/* #236 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554477
	/* java_name */
	.ascii	"android/support/v7/widget/DecorToolbar"
	.zero	51
	.zero	1

	/* #237 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"android/support/v7/widget/ScrollingTabContainerView"
	.zero	38
	.zero	1

	/* #238 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554479
	/* java_name */
	.ascii	"android/support/v7/widget/ScrollingTabContainerView$VisibilityAnimListener"
	.zero	15
	.zero	1

	/* #239 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554480
	/* java_name */
	.ascii	"android/support/v7/widget/SwitchCompat"
	.zero	51
	.zero	1

	/* #240 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554466
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar"
	.zero	56
	.zero	1

	/* #241 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar$OnMenuItemClickListener"
	.zero	32
	.zero	1

	/* #242 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar_NavigationOnClickEventDispatcher"
	.zero	23
	.zero	1

	/* #243 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/Editable"
	.zero	68
	.zero	1

	/* #244 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/GetChars"
	.zero	68
	.zero	1

	/* #245 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/InputFilter"
	.zero	65
	.zero	1

	/* #246 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554902
	/* java_name */
	.ascii	"android/text/InputFilter$AllCaps"
	.zero	57
	.zero	1

	/* #247 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554916
	/* java_name */
	.ascii	"android/text/Layout"
	.zero	70
	.zero	1

	/* #248 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554917
	/* java_name */
	.ascii	"android/text/Layout$Alignment"
	.zero	60
	.zero	1

	/* #249 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/NoCopySpan"
	.zero	66
	.zero	1

	/* #250 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/Spannable"
	.zero	67
	.zero	1

	/* #251 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/Spanned"
	.zero	69
	.zero	1

	/* #252 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554920
	/* java_name */
	.ascii	"android/text/StaticLayout"
	.zero	64
	.zero	1

	/* #253 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554921
	/* java_name */
	.ascii	"android/text/TextPaint"
	.zero	67
	.zero	1

	/* #254 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/text/TextWatcher"
	.zero	65
	.zero	1

	/* #255 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/util/AttributeSet"
	.zero	64
	.zero	1

	/* #256 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554889
	/* java_name */
	.ascii	"android/util/DisplayMetrics"
	.zero	62
	.zero	1

	/* #257 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554887
	/* java_name */
	.ascii	"android/util/Log"
	.zero	73
	.zero	1

	/* #258 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554892
	/* java_name */
	.ascii	"android/util/SparseArray"
	.zero	65
	.zero	1

	/* #259 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554787
	/* java_name */
	.ascii	"android/view/ActionMode"
	.zero	66
	.zero	1

	/* #260 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ActionMode$Callback"
	.zero	57
	.zero	1

	/* #261 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554792
	/* java_name */
	.ascii	"android/view/ActionProvider"
	.zero	62
	.zero	1

	/* #262 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ContextMenu"
	.zero	65
	.zero	1

	/* #263 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ContextMenu$ContextMenuInfo"
	.zero	49
	.zero	1

	/* #264 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554794
	/* java_name */
	.ascii	"android/view/ContextThemeWrapper"
	.zero	57
	.zero	1

	/* #265 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554795
	/* java_name */
	.ascii	"android/view/Display"
	.zero	69
	.zero	1

	/* #266 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554796
	/* java_name */
	.ascii	"android/view/DragEvent"
	.zero	67
	.zero	1

	/* #267 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554798
	/* java_name */
	.ascii	"android/view/GestureDetector"
	.zero	61
	.zero	1

	/* #268 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/GestureDetector$OnContextClickListener"
	.zero	38
	.zero	1

	/* #269 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/GestureDetector$OnDoubleTapListener"
	.zero	41
	.zero	1

	/* #270 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/GestureDetector$OnGestureListener"
	.zero	43
	.zero	1

	/* #271 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554810
	/* java_name */
	.ascii	"android/view/GestureDetector$SimpleOnGestureListener"
	.zero	37
	.zero	1

	/* #272 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554827
	/* java_name */
	.ascii	"android/view/InputEvent"
	.zero	66
	.zero	1

	/* #273 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554766
	/* java_name */
	.ascii	"android/view/KeyEvent"
	.zero	68
	.zero	1

	/* #274 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/KeyEvent$Callback"
	.zero	59
	.zero	1

	/* #275 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554769
	/* java_name */
	.ascii	"android/view/LayoutInflater"
	.zero	62
	.zero	1

	/* #276 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/LayoutInflater$Factory"
	.zero	54
	.zero	1

	/* #277 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/LayoutInflater$Factory2"
	.zero	53
	.zero	1

	/* #278 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/LayoutInflater$Filter"
	.zero	55
	.zero	1

	/* #279 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/Menu"
	.zero	72
	.zero	1

	/* #280 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554848
	/* java_name */
	.ascii	"android/view/MenuInflater"
	.zero	64
	.zero	1

	/* #281 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/MenuItem"
	.zero	68
	.zero	1

	/* #282 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/MenuItem$OnActionExpandListener"
	.zero	45
	.zero	1

	/* #283 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/MenuItem$OnMenuItemClickListener"
	.zero	44
	.zero	1

	/* #284 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554776
	/* java_name */
	.ascii	"android/view/MotionEvent"
	.zero	65
	.zero	1

	/* #285 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554852
	/* java_name */
	.ascii	"android/view/SearchEvent"
	.zero	65
	.zero	1

	/* #286 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/SubMenu"
	.zero	69
	.zero	1

	/* #287 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554855
	/* java_name */
	.ascii	"android/view/Surface"
	.zero	69
	.zero	1

	/* #288 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/SurfaceHolder"
	.zero	63
	.zero	1

	/* #289 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/SurfaceHolder$Callback"
	.zero	54
	.zero	1

	/* #290 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/SurfaceHolder$Callback2"
	.zero	53
	.zero	1

	/* #291 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554858
	/* java_name */
	.ascii	"android/view/SurfaceView"
	.zero	65
	.zero	1

	/* #292 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554861
	/* java_name */
	.ascii	"android/view/TextureView"
	.zero	65
	.zero	1

	/* #293 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/TextureView$SurfaceTextureListener"
	.zero	42
	.zero	1

	/* #294 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554732
	/* java_name */
	.ascii	"android/view/View"
	.zero	72
	.zero	1

	/* #295 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnClickListener"
	.zero	56
	.zero	1

	/* #296 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnCreateContextMenuListener"
	.zero	44
	.zero	1

	/* #297 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnFocusChangeListener"
	.zero	50
	.zero	1

	/* #298 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnKeyListener"
	.zero	58
	.zero	1

	/* #299 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnLayoutChangeListener"
	.zero	49
	.zero	1

	/* #300 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnLongClickListener"
	.zero	52
	.zero	1

	/* #301 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/View$OnTouchListener"
	.zero	56
	.zero	1

	/* #302 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554864
	/* java_name */
	.ascii	"android/view/ViewGroup"
	.zero	67
	.zero	1

	/* #303 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554865
	/* java_name */
	.ascii	"android/view/ViewGroup$LayoutParams"
	.zero	54
	.zero	1

	/* #304 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554866
	/* java_name */
	.ascii	"android/view/ViewGroup$MarginLayoutParams"
	.zero	48
	.zero	1

	/* #305 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ViewManager"
	.zero	65
	.zero	1

	/* #306 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ViewParent"
	.zero	66
	.zero	1

	/* #307 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554868
	/* java_name */
	.ascii	"android/view/ViewPropertyAnimator"
	.zero	56
	.zero	1

	/* #308 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554777
	/* java_name */
	.ascii	"android/view/ViewTreeObserver"
	.zero	60
	.zero	1

	/* #309 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnGlobalLayoutListener"
	.zero	37
	.zero	1

	/* #310 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnPreDrawListener"
	.zero	42
	.zero	1

	/* #311 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnTouchModeChangeListener"
	.zero	34
	.zero	1

	/* #312 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554784
	/* java_name */
	.ascii	"android/view/Window"
	.zero	70
	.zero	1

	/* #313 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/Window$Callback"
	.zero	61
	.zero	1

	/* #314 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/WindowManager"
	.zero	63
	.zero	1

	/* #315 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554841
	/* java_name */
	.ascii	"android/view/WindowManager$LayoutParams"
	.zero	50
	.zero	1

	/* #316 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554880
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityEvent"
	.zero	44
	.zero	1

	/* #317 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityEventSource"
	.zero	38
	.zero	1

	/* #318 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554881
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityRecord"
	.zero	43
	.zero	1

	/* #319 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554873
	/* java_name */
	.ascii	"android/view/animation/Animation"
	.zero	57
	.zero	1

	/* #320 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/view/animation/Interpolator"
	.zero	54
	.zero	1

	/* #321 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554877
	/* java_name */
	.ascii	"android/view/inputmethod/InputMethodManager"
	.zero	46
	.zero	1

	/* #322 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554656
	/* java_name */
	.ascii	"android/widget/AbsListView"
	.zero	63
	.zero	1

	/* #323 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/Adapter"
	.zero	67
	.zero	1

	/* #324 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554658
	/* java_name */
	.ascii	"android/widget/AdapterView"
	.zero	63
	.zero	1

	/* #325 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemClickListener"
	.zero	43
	.zero	1

	/* #326 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemLongClickListener"
	.zero	39
	.zero	1

	/* #327 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemSelectedListener"
	.zero	40
	.zero	1

	/* #328 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/ArrayAdapter"
	.zero	62
	.zero	1

	/* #329 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/BaseAdapter"
	.zero	63
	.zero	1

	/* #330 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554684
	/* java_name */
	.ascii	"android/widget/Button"
	.zero	68
	.zero	1

	/* #331 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554685
	/* java_name */
	.ascii	"android/widget/CheckBox"
	.zero	66
	.zero	1

	/* #332 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/Checkable"
	.zero	65
	.zero	1

	/* #333 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554687
	/* java_name */
	.ascii	"android/widget/CompoundButton"
	.zero	60
	.zero	1

	/* #334 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/CompoundButton$OnCheckedChangeListener"
	.zero	36
	.zero	1

	/* #335 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554676
	/* java_name */
	.ascii	"android/widget/DatePicker"
	.zero	64
	.zero	1

	/* #336 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/DatePicker$OnDateChangedListener"
	.zero	42
	.zero	1

	/* #337 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554695
	/* java_name */
	.ascii	"android/widget/EditText"
	.zero	66
	.zero	1

	/* #338 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554696
	/* java_name */
	.ascii	"android/widget/Filter"
	.zero	68
	.zero	1

	/* #339 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/Filter$FilterListener"
	.zero	53
	.zero	1

	/* #340 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/Filterable"
	.zero	64
	.zero	1

	/* #341 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554700
	/* java_name */
	.ascii	"android/widget/FrameLayout"
	.zero	63
	.zero	1

	/* #342 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554701
	/* java_name */
	.ascii	"android/widget/HorizontalScrollView"
	.zero	54
	.zero	1

	/* #343 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554710
	/* java_name */
	.ascii	"android/widget/ImageButton"
	.zero	63
	.zero	1

	/* #344 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554711
	/* java_name */
	.ascii	"android/widget/ImageView"
	.zero	65
	.zero	1

	/* #345 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554716
	/* java_name */
	.ascii	"android/widget/LinearLayout"
	.zero	62
	.zero	1

	/* #346 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554717
	/* java_name */
	.ascii	"android/widget/LinearLayout$LayoutParams"
	.zero	49
	.zero	1

	/* #347 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/ListAdapter"
	.zero	63
	.zero	1

	/* #348 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554718
	/* java_name */
	.ascii	"android/widget/ListView"
	.zero	66
	.zero	1

	/* #349 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554719
	/* java_name */
	.ascii	"android/widget/ProgressBar"
	.zero	63
	.zero	1

	/* #350 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554720
	/* java_name */
	.ascii	"android/widget/RadioButton"
	.zero	63
	.zero	1

	/* #351 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554721
	/* java_name */
	.ascii	"android/widget/RadioGroup"
	.zero	64
	.zero	1

	/* #352 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/RadioGroup$OnCheckedChangeListener"
	.zero	40
	.zero	1

	/* #353 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554728
	/* java_name */
	.ascii	"android/widget/RelativeLayout"
	.zero	60
	.zero	1

	/* #354 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554729
	/* java_name */
	.ascii	"android/widget/RemoteViews"
	.zero	63
	.zero	1

	/* #355 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/SpinnerAdapter"
	.zero	60
	.zero	1

	/* #356 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554679
	/* java_name */
	.ascii	"android/widget/TextView"
	.zero	66
	.zero	1

	/* #357 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/ThemedSpinnerAdapter"
	.zero	54
	.zero	1

	/* #358 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554730
	/* java_name */
	.ascii	"android/widget/Toast"
	.zero	69
	.zero	1

	/* #359 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"androidx/work/BackoffPolicy"
	.zero	62
	.zero	1

	/* #360 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"androidx/work/Configuration"
	.zero	62
	.zero	1

	/* #361 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"androidx/work/Constraints"
	.zero	64
	.zero	1

	/* #362 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"androidx/work/ContentUriTriggers"
	.zero	57
	.zero	1

	/* #363 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"androidx/work/ContentUriTriggers$Trigger"
	.zero	49
	.zero	1

	/* #364 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"androidx/work/Data"
	.zero	71
	.zero	1

	/* #365 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"androidx/work/ExistingPeriodicWorkPolicy"
	.zero	49
	.zero	1

	/* #366 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"androidx/work/ExistingWorkPolicy"
	.zero	57
	.zero	1

	/* #367 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"androidx/work/ListenableWorker"
	.zero	59
	.zero	1

	/* #368 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"androidx/work/ListenableWorker$Result"
	.zero	52
	.zero	1

	/* #369 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"androidx/work/NetworkType"
	.zero	64
	.zero	1

	/* #370 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"androidx/work/OneTimeWorkRequest"
	.zero	57
	.zero	1

	/* #371 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"androidx/work/OneTimeWorkRequest$Builder"
	.zero	49
	.zero	1

	/* #372 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"androidx/work/Operation"
	.zero	66
	.zero	1

	/* #373 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"androidx/work/PeriodicWorkRequest"
	.zero	56
	.zero	1

	/* #374 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"androidx/work/PeriodicWorkRequest$Builder"
	.zero	48
	.zero	1

	/* #375 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"androidx/work/WorkContinuation"
	.zero	59
	.zero	1

	/* #376 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"androidx/work/WorkInfo"
	.zero	67
	.zero	1

	/* #377 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"androidx/work/WorkInfo$State"
	.zero	61
	.zero	1

	/* #378 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"androidx/work/WorkManager"
	.zero	64
	.zero	1

	/* #379 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"androidx/work/WorkRequest"
	.zero	64
	.zero	1

	/* #380 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"androidx/work/WorkRequest$Builder"
	.zero	56
	.zero	1

	/* #381 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"androidx/work/Worker"
	.zero	69
	.zero	1

	/* #382 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"androidx/work/WorkerFactory"
	.zero	62
	.zero	1

	/* #383 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554450
	/* java_name */
	.ascii	"androidx/work/WorkerParameters"
	.zero	59
	.zero	1

	/* #384 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"crc6414d91685051e871f/PdfCreator"
	.zero	57
	.zero	1

	/* #385 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"crc641b9169a4674060ee/ZXingScannerFragment"
	.zero	47
	.zero	1

	/* #386 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"crc641b9169a4674060ee/ZXingSurfaceView"
	.zero	51
	.zero	1

	/* #387 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"crc641b9169a4674060ee/ZxingActivity"
	.zero	54
	.zero	1

	/* #388 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"crc641b9169a4674060ee/ZxingOverlayView"
	.zero	51
	.zero	1

	/* #389 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"crc6432a82514f08b9405/AgendaView"
	.zero	57
	.zero	1

	/* #390 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscaClienteView"
	.zero	51
	.zero	1

	/* #391 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscaMunicipioView"
	.zero	49
	.zero	1

	/* #392 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscarClientesBaixaView"
	.zero	44
	.zero	1

	/* #393 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscarPedidoDevolucao"
	.zero	46
	.zero	1

	/* #394 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscarPedidoView"
	.zero	51
	.zero	1

	/* #395 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"crc6432a82514f08b9405/BuscarProdutoView"
	.zero	50
	.zero	1

	/* #396 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"crc6432a82514f08b9405/ClienteView"
	.zero	56
	.zero	1

	/* #397 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"crc6432a82514f08b9405/ConfigView"
	.zero	57
	.zero	1

	/* #398 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"crc6432a82514f08b9405/DevolucaoPedidoView"
	.zero	48
	.zero	1

	/* #399 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"crc6432a82514f08b9405/DevolucaoPedidoViewOld"
	.zero	45
	.zero	1

	/* #400 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554450
	/* java_name */
	.ascii	"crc6432a82514f08b9405/EstoqueView"
	.zero	56
	.zero	1

	/* #401 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"crc6432a82514f08b9405/LoginView"
	.zero	58
	.zero	1

	/* #402 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"crc6432a82514f08b9405/PedidoView"
	.zero	57
	.zero	1

	/* #403 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"crc6432a82514f08b9405/RelatorioBaixasView"
	.zero	48
	.zero	1

	/* #404 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"crc6432a82514f08b9405/RelatorioDevolucoesView"
	.zero	44
	.zero	1

	/* #405 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"crc6432a82514f08b9405/RelatorioEmissaoView"
	.zero	47
	.zero	1

	/* #406 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"crc6432a82514f08b9405/RelatorioRomaneioView"
	.zero	46
	.zero	1

	/* #407 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"crc6432a82514f08b9405/ResumoPedidoView"
	.zero	51
	.zero	1

	/* #408 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"crc6432a82514f08b9405/VendedorView"
	.zero	55
	.zero	1

	/* #409 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"crc6432a82514f08b9405/gMapView"
	.zero	59
	.zero	1

	/* #410 */
	/* module_index */
	.long	19
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"crc644032a24cb306f3fa/MapControl"
	.zero	57
	.zero	1

	/* #411 */
	/* module_index */
	.long	19
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"crc644032a24cb306f3fa/MapControlGestureListener"
	.zero	42
	.zero	1

	/* #412 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"crc64435a5ac349fa9fda/ActivityLifecycleContextListener"
	.zero	35
	.zero	1

	/* #413 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"crc645624b9e5377aa8fe/GeolocatorBroadCast"
	.zero	48
	.zero	1

	/* #414 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"crc645624b9e5377aa8fe/GeolocatorWork"
	.zero	53
	.zero	1

	/* #415 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554473
	/* java_name */
	.ascii	"crc647cff354ad15604ee/FileProviderClass"
	.zero	50
	.zero	1

	/* #416 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"crc6487a259cee5d6aa49/MainActivity"
	.zero	55
	.zero	1

	/* #417 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"crc648e35430423bd4943/GLTextureView"
	.zero	54
	.zero	1

	/* #418 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554450
	/* java_name */
	.ascii	"crc648e35430423bd4943/GLTextureView_LogWriter"
	.zero	44
	.zero	1

	/* #419 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKCanvasView"
	.zero	55
	.zero	1

	/* #420 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLSurfaceView"
	.zero	52
	.zero	1

	/* #421 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLSurfaceViewRenderer"
	.zero	44
	.zero	1

	/* #422 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLSurfaceView_InternalRenderer"
	.zero	35
	.zero	1

	/* #423 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLTextureView"
	.zero	52
	.zero	1

	/* #424 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLTextureViewRenderer"
	.zero	44
	.zero	1

	/* #425 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKGLTextureView_InternalRenderer"
	.zero	35
	.zero	1

	/* #426 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"crc648e35430423bd4943/SKSurfaceView"
	.zero	54
	.zero	1

	/* #427 */
	/* module_index */
	.long	7
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"crc649efb5bdbf2d8cfa5/GeolocationContinuousListener"
	.zero	38
	.zero	1

	/* #428 */
	/* module_index */
	.long	7
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"crc649efb5bdbf2d8cfa5/GeolocationSingleListener"
	.zero	42
	.zero	1

	/* #429 */
	/* module_index */
	.long	20
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"crc64a0e0a82d0db9a07d/ActivityLifecycleContextListener"
	.zero	35
	.zero	1

	/* #430 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554538
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterBuscarCliente"
	.zero	47
	.zero	1

	/* #431 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554542
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterBuscarPedidos"
	.zero	47
	.zero	1

	/* #432 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554539
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterBuscarProduto"
	.zero	47
	.zero	1

	/* #433 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554540
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterItensDevolucao"
	.zero	46
	.zero	1

	/* #434 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554541
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterItensPedido"
	.zero	49
	.zero	1

	/* #435 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554543
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterListaBaixas"
	.zero	49
	.zero	1

	/* #436 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554544
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AdapterListaPedidos"
	.zero	48
	.zero	1

	/* #437 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554545
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/AgendaAdapter"
	.zero	54
	.zero	1

	/* #438 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554546
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/BaixaPedidoAdapter"
	.zero	49
	.zero	1

	/* #439 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554547
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/DevolucaoItensAdapter"
	.zero	46
	.zero	1

	/* #440 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554548
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/EmissaoRelatorioAdapter"
	.zero	44
	.zero	1

	/* #441 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554549
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/EstoqueAdapter"
	.zero	53
	.zero	1

	/* #442 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554550
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/RelatorioBaixasAdapter"
	.zero	45
	.zero	1

	/* #443 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554551
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/RelatorioDevolucoesAdapter"
	.zero	41
	.zero	1

	/* #444 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554552
	/* java_name */
	.ascii	"crc64ae056d6e2660918a/RelatorioRomaneioAdapter"
	.zero	43
	.zero	1

	/* #445 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"crc64e9f97cf19b8286a9/ChartView"
	.zero	58
	.zero	1

	/* #446 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/io/Closeable"
	.zero	72
	.zero	1

	/* #447 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555337
	/* java_name */
	.ascii	"java/io/File"
	.zero	77
	.zero	1

	/* #448 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555338
	/* java_name */
	.ascii	"java/io/FileDescriptor"
	.zero	67
	.zero	1

	/* #449 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555339
	/* java_name */
	.ascii	"java/io/FileInputStream"
	.zero	66
	.zero	1

	/* #450 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555340
	/* java_name */
	.ascii	"java/io/FileOutputStream"
	.zero	65
	.zero	1

	/* #451 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/io/Flushable"
	.zero	72
	.zero	1

	/* #452 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555348
	/* java_name */
	.ascii	"java/io/IOException"
	.zero	70
	.zero	1

	/* #453 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555345
	/* java_name */
	.ascii	"java/io/InputStream"
	.zero	70
	.zero	1

	/* #454 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555347
	/* java_name */
	.ascii	"java/io/InterruptedIOException"
	.zero	59
	.zero	1

	/* #455 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555351
	/* java_name */
	.ascii	"java/io/OutputStream"
	.zero	69
	.zero	1

	/* #456 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555353
	/* java_name */
	.ascii	"java/io/PrintWriter"
	.zero	70
	.zero	1

	/* #457 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/io/Serializable"
	.zero	69
	.zero	1

	/* #458 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555354
	/* java_name */
	.ascii	"java/io/StringWriter"
	.zero	69
	.zero	1

	/* #459 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555355
	/* java_name */
	.ascii	"java/io/Writer"
	.zero	75
	.zero	1

	/* #460 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555289
	/* java_name */
	.ascii	"java/lang/AbstractStringBuilder"
	.zero	58
	.zero	1

	/* #461 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/Appendable"
	.zero	69
	.zero	1

	/* #462 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/AutoCloseable"
	.zero	66
	.zero	1

	/* #463 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555265
	/* java_name */
	.ascii	"java/lang/Boolean"
	.zero	72
	.zero	1

	/* #464 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555266
	/* java_name */
	.ascii	"java/lang/Byte"
	.zero	75
	.zero	1

	/* #465 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/CharSequence"
	.zero	67
	.zero	1

	/* #466 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555267
	/* java_name */
	.ascii	"java/lang/Character"
	.zero	70
	.zero	1

	/* #467 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555268
	/* java_name */
	.ascii	"java/lang/Class"
	.zero	74
	.zero	1

	/* #468 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555292
	/* java_name */
	.ascii	"java/lang/ClassCastException"
	.zero	61
	.zero	1

	/* #469 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555269
	/* java_name */
	.ascii	"java/lang/ClassNotFoundException"
	.zero	57
	.zero	1

	/* #470 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/Cloneable"
	.zero	70
	.zero	1

	/* #471 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/Comparable"
	.zero	69
	.zero	1

	/* #472 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555270
	/* java_name */
	.ascii	"java/lang/Double"
	.zero	73
	.zero	1

	/* #473 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555293
	/* java_name */
	.ascii	"java/lang/Enum"
	.zero	75
	.zero	1

	/* #474 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555295
	/* java_name */
	.ascii	"java/lang/Error"
	.zero	74
	.zero	1

	/* #475 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555271
	/* java_name */
	.ascii	"java/lang/Exception"
	.zero	70
	.zero	1

	/* #476 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555272
	/* java_name */
	.ascii	"java/lang/Float"
	.zero	74
	.zero	1

	/* #477 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555308
	/* java_name */
	.ascii	"java/lang/IllegalArgumentException"
	.zero	55
	.zero	1

	/* #478 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555309
	/* java_name */
	.ascii	"java/lang/IllegalStateException"
	.zero	58
	.zero	1

	/* #479 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555310
	/* java_name */
	.ascii	"java/lang/IndexOutOfBoundsException"
	.zero	54
	.zero	1

	/* #480 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555274
	/* java_name */
	.ascii	"java/lang/Integer"
	.zero	72
	.zero	1

	/* #481 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/Iterable"
	.zero	71
	.zero	1

	/* #482 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555313
	/* java_name */
	.ascii	"java/lang/LinkageError"
	.zero	67
	.zero	1

	/* #483 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555275
	/* java_name */
	.ascii	"java/lang/Long"
	.zero	75
	.zero	1

	/* #484 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555314
	/* java_name */
	.ascii	"java/lang/NoClassDefFoundError"
	.zero	59
	.zero	1

	/* #485 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555315
	/* java_name */
	.ascii	"java/lang/NullPointerException"
	.zero	59
	.zero	1

	/* #486 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555316
	/* java_name */
	.ascii	"java/lang/Number"
	.zero	73
	.zero	1

	/* #487 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555276
	/* java_name */
	.ascii	"java/lang/Object"
	.zero	73
	.zero	1

	/* #488 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555318
	/* java_name */
	.ascii	"java/lang/ReflectiveOperationException"
	.zero	51
	.zero	1

	/* #489 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/Runnable"
	.zero	71
	.zero	1

	/* #490 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555278
	/* java_name */
	.ascii	"java/lang/RuntimeException"
	.zero	63
	.zero	1

	/* #491 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555319
	/* java_name */
	.ascii	"java/lang/SecurityException"
	.zero	62
	.zero	1

	/* #492 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555279
	/* java_name */
	.ascii	"java/lang/Short"
	.zero	74
	.zero	1

	/* #493 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555280
	/* java_name */
	.ascii	"java/lang/String"
	.zero	73
	.zero	1

	/* #494 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555282
	/* java_name */
	.ascii	"java/lang/StringBuffer"
	.zero	67
	.zero	1

	/* #495 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555284
	/* java_name */
	.ascii	"java/lang/StringBuilder"
	.zero	66
	.zero	1

	/* #496 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555286
	/* java_name */
	.ascii	"java/lang/Thread"
	.zero	73
	.zero	1

	/* #497 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555288
	/* java_name */
	.ascii	"java/lang/Throwable"
	.zero	70
	.zero	1

	/* #498 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555320
	/* java_name */
	.ascii	"java/lang/UnsupportedOperationException"
	.zero	50
	.zero	1

	/* #499 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/annotation/Annotation"
	.zero	58
	.zero	1

	/* #500 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555323
	/* java_name */
	.ascii	"java/lang/reflect/AccessibleObject"
	.zero	55
	.zero	1

	/* #501 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/reflect/AnnotatedElement"
	.zero	55
	.zero	1

	/* #502 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555324
	/* java_name */
	.ascii	"java/lang/reflect/Executable"
	.zero	61
	.zero	1

	/* #503 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/reflect/GenericDeclaration"
	.zero	53
	.zero	1

	/* #504 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/reflect/Member"
	.zero	65
	.zero	1

	/* #505 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555336
	/* java_name */
	.ascii	"java/lang/reflect/Method"
	.zero	65
	.zero	1

	/* #506 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/reflect/Type"
	.zero	67
	.zero	1

	/* #507 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/lang/reflect/TypeVariable"
	.zero	59
	.zero	1

	/* #508 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555190
	/* java_name */
	.ascii	"java/net/ConnectException"
	.zero	64
	.zero	1

	/* #509 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555192
	/* java_name */
	.ascii	"java/net/HttpURLConnection"
	.zero	63
	.zero	1

	/* #510 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555194
	/* java_name */
	.ascii	"java/net/InetSocketAddress"
	.zero	63
	.zero	1

	/* #511 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555195
	/* java_name */
	.ascii	"java/net/ProtocolException"
	.zero	63
	.zero	1

	/* #512 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555196
	/* java_name */
	.ascii	"java/net/Proxy"
	.zero	75
	.zero	1

	/* #513 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555197
	/* java_name */
	.ascii	"java/net/Proxy$Type"
	.zero	70
	.zero	1

	/* #514 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555198
	/* java_name */
	.ascii	"java/net/ProxySelector"
	.zero	67
	.zero	1

	/* #515 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555200
	/* java_name */
	.ascii	"java/net/SocketAddress"
	.zero	67
	.zero	1

	/* #516 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555202
	/* java_name */
	.ascii	"java/net/SocketException"
	.zero	65
	.zero	1

	/* #517 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555203
	/* java_name */
	.ascii	"java/net/SocketTimeoutException"
	.zero	58
	.zero	1

	/* #518 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555205
	/* java_name */
	.ascii	"java/net/URI"
	.zero	77
	.zero	1

	/* #519 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555206
	/* java_name */
	.ascii	"java/net/URL"
	.zero	77
	.zero	1

	/* #520 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555207
	/* java_name */
	.ascii	"java/net/URLConnection"
	.zero	67
	.zero	1

	/* #521 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555204
	/* java_name */
	.ascii	"java/net/UnknownServiceException"
	.zero	57
	.zero	1

	/* #522 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555237
	/* java_name */
	.ascii	"java/nio/Buffer"
	.zero	74
	.zero	1

	/* #523 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555239
	/* java_name */
	.ascii	"java/nio/ByteBuffer"
	.zero	70
	.zero	1

	/* #524 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555241
	/* java_name */
	.ascii	"java/nio/FloatBuffer"
	.zero	69
	.zero	1

	/* #525 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555243
	/* java_name */
	.ascii	"java/nio/IntBuffer"
	.zero	71
	.zero	1

	/* #526 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/ByteChannel"
	.zero	60
	.zero	1

	/* #527 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/Channel"
	.zero	64
	.zero	1

	/* #528 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555245
	/* java_name */
	.ascii	"java/nio/channels/FileChannel"
	.zero	60
	.zero	1

	/* #529 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/GatheringByteChannel"
	.zero	51
	.zero	1

	/* #530 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/InterruptibleChannel"
	.zero	51
	.zero	1

	/* #531 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/ReadableByteChannel"
	.zero	52
	.zero	1

	/* #532 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/ScatteringByteChannel"
	.zero	50
	.zero	1

	/* #533 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/SeekableByteChannel"
	.zero	52
	.zero	1

	/* #534 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/nio/channels/WritableByteChannel"
	.zero	52
	.zero	1

	/* #535 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555263
	/* java_name */
	.ascii	"java/nio/channels/spi/AbstractInterruptibleChannel"
	.zero	39
	.zero	1

	/* #536 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555220
	/* java_name */
	.ascii	"java/security/KeyStore"
	.zero	67
	.zero	1

	/* #537 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/security/KeyStore$LoadStoreParameter"
	.zero	48
	.zero	1

	/* #538 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/security/KeyStore$ProtectionParameter"
	.zero	47
	.zero	1

	/* #539 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555225
	/* java_name */
	.ascii	"java/security/MessageDigest"
	.zero	62
	.zero	1

	/* #540 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555227
	/* java_name */
	.ascii	"java/security/MessageDigestSpi"
	.zero	59
	.zero	1

	/* #541 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/security/Principal"
	.zero	66
	.zero	1

	/* #542 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555229
	/* java_name */
	.ascii	"java/security/SecureRandom"
	.zero	63
	.zero	1

	/* #543 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555230
	/* java_name */
	.ascii	"java/security/cert/Certificate"
	.zero	59
	.zero	1

	/* #544 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555232
	/* java_name */
	.ascii	"java/security/cert/CertificateFactory"
	.zero	52
	.zero	1

	/* #545 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555235
	/* java_name */
	.ascii	"java/security/cert/X509Certificate"
	.zero	55
	.zero	1

	/* #546 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/security/cert/X509Extension"
	.zero	57
	.zero	1

	/* #547 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555158
	/* java_name */
	.ascii	"java/util/ArrayList"
	.zero	70
	.zero	1

	/* #548 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555147
	/* java_name */
	.ascii	"java/util/Collection"
	.zero	69
	.zero	1

	/* #549 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/util/Enumeration"
	.zero	68
	.zero	1

	/* #550 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555149
	/* java_name */
	.ascii	"java/util/HashMap"
	.zero	72
	.zero	1

	/* #551 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555167
	/* java_name */
	.ascii	"java/util/HashSet"
	.zero	72
	.zero	1

	/* #552 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/util/Iterator"
	.zero	71
	.zero	1

	/* #553 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555213
	/* java_name */
	.ascii	"java/util/Random"
	.zero	73
	.zero	1

	/* #554 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555214
	/* java_name */
	.ascii	"java/util/UUID"
	.zero	75
	.zero	1

	/* #555 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"java/util/concurrent/Executor"
	.zero	60
	.zero	1

	/* #556 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555217
	/* java_name */
	.ascii	"java/util/concurrent/TimeUnit"
	.zero	60
	.zero	1

	/* #557 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGL"
	.zero	55
	.zero	1

	/* #558 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554633
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGL10"
	.zero	53
	.zero	1

	/* #559 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554624
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGLConfig"
	.zero	49
	.zero	1

	/* #560 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554623
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGLContext"
	.zero	48
	.zero	1

	/* #561 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554627
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGLDisplay"
	.zero	48
	.zero	1

	/* #562 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554629
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGLSurface"
	.zero	48
	.zero	1

	/* #563 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/microedition/khronos/opengles/GL"
	.zero	51
	.zero	1

	/* #564 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/microedition/khronos/opengles/GL10"
	.zero	49
	.zero	1

	/* #565 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554598
	/* java_name */
	.ascii	"javax/net/SocketFactory"
	.zero	66
	.zero	1

	/* #566 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/HostnameVerifier"
	.zero	59
	.zero	1

	/* #567 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554600
	/* java_name */
	.ascii	"javax/net/ssl/HttpsURLConnection"
	.zero	57
	.zero	1

	/* #568 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/KeyManager"
	.zero	65
	.zero	1

	/* #569 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554614
	/* java_name */
	.ascii	"javax/net/ssl/KeyManagerFactory"
	.zero	58
	.zero	1

	/* #570 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554615
	/* java_name */
	.ascii	"javax/net/ssl/SSLContext"
	.zero	65
	.zero	1

	/* #571 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/SSLSession"
	.zero	65
	.zero	1

	/* #572 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/SSLSessionContext"
	.zero	58
	.zero	1

	/* #573 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554616
	/* java_name */
	.ascii	"javax/net/ssl/SSLSocketFactory"
	.zero	59
	.zero	1

	/* #574 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/TrustManager"
	.zero	63
	.zero	1

	/* #575 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554618
	/* java_name */
	.ascii	"javax/net/ssl/TrustManagerFactory"
	.zero	56
	.zero	1

	/* #576 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"javax/net/ssl/X509TrustManager"
	.zero	59
	.zero	1

	/* #577 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554594
	/* java_name */
	.ascii	"javax/security/cert/Certificate"
	.zero	58
	.zero	1

	/* #578 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554596
	/* java_name */
	.ascii	"javax/security/cert/X509Certificate"
	.zero	54
	.zero	1

	/* #579 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555378
	/* java_name */
	.ascii	"mono/android/TypeManager"
	.zero	65
	.zero	1

	/* #580 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555040
	/* java_name */
	.ascii	"mono/android/app/DatePickerDialog_OnDateSetListenerImplementor"
	.zero	27
	.zero	1

	/* #581 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555086
	/* java_name */
	.ascii	"mono/android/content/DialogInterface_OnClickListenerImplementor"
	.zero	26
	.zero	1

	/* #582 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555143
	/* java_name */
	.ascii	"mono/android/runtime/InputStreamAdapter"
	.zero	50
	.zero	1

	/* #583 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"mono/android/runtime/JavaArray"
	.zero	59
	.zero	1

	/* #584 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555164
	/* java_name */
	.ascii	"mono/android/runtime/JavaObject"
	.zero	58
	.zero	1

	/* #585 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555182
	/* java_name */
	.ascii	"mono/android/runtime/OutputStreamAdapter"
	.zero	49
	.zero	1

	/* #586 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"mono/android/support/design/widget/SwipeDismissBehavior_OnDismissListenerImplementor"
	.zero	5
	.zero	1

	/* #587 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"mono/android/support/v4/app/FragmentManager_OnBackStackChangedListenerImplementor"
	.zero	8
	.zero	1

	/* #588 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"mono/android/support/v4/view/ActionProvider_SubUiVisibilityListenerImplementor"
	.zero	11
	.zero	1

	/* #589 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"mono/android/support/v4/view/ActionProvider_VisibilityListenerImplementor"
	.zero	16
	.zero	1

	/* #590 */
	/* module_index */
	.long	17
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"mono/android/support/v4/widget/DrawerLayout_DrawerListenerImplementor"
	.zero	20
	.zero	1

	/* #591 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"mono/android/support/v7/app/ActionBar_OnMenuVisibilityListenerImplementor"
	.zero	16
	.zero	1

	/* #592 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"mono/android/support/v7/widget/Toolbar_OnMenuItemClickListenerImplementor"
	.zero	16
	.zero	1

	/* #593 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554895
	/* java_name */
	.ascii	"mono/android/text/TextWatcherImplementor"
	.zero	49
	.zero	1

	/* #594 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554806
	/* java_name */
	.ascii	"mono/android/view/GestureDetector_OnDoubleTapListenerImplementor"
	.zero	25
	.zero	1

	/* #595 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554735
	/* java_name */
	.ascii	"mono/android/view/View_OnClickListenerImplementor"
	.zero	40
	.zero	1

	/* #596 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554741
	/* java_name */
	.ascii	"mono/android/view/View_OnFocusChangeListenerImplementor"
	.zero	34
	.zero	1

	/* #597 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554745
	/* java_name */
	.ascii	"mono/android/view/View_OnKeyListenerImplementor"
	.zero	42
	.zero	1

	/* #598 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554751
	/* java_name */
	.ascii	"mono/android/view/View_OnLongClickListenerImplementor"
	.zero	36
	.zero	1

	/* #599 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554755
	/* java_name */
	.ascii	"mono/android/view/View_OnTouchListenerImplementor"
	.zero	40
	.zero	1

	/* #600 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554662
	/* java_name */
	.ascii	"mono/android/widget/AdapterView_OnItemClickListenerImplementor"
	.zero	27
	.zero	1

	/* #601 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554666
	/* java_name */
	.ascii	"mono/android/widget/AdapterView_OnItemLongClickListenerImplementor"
	.zero	23
	.zero	1

	/* #602 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554691
	/* java_name */
	.ascii	"mono/android/widget/CompoundButton_OnCheckedChangeListenerImplementor"
	.zero	20
	.zero	1

	/* #603 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554725
	/* java_name */
	.ascii	"mono/android/widget/RadioGroup_OnCheckedChangeListenerImplementor"
	.zero	24
	.zero	1

	/* #604 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555277
	/* java_name */
	.ascii	"mono/java/lang/Runnable"
	.zero	66
	.zero	1

	/* #605 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555287
	/* java_name */
	.ascii	"mono/java/lang/RunnableImplementor"
	.zero	55
	.zero	1

	/* #606 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554593
	/* java_name */
	.ascii	"xamarin/android/net/OldAndroidSSLSocketFactory"
	.zero	43
	.zero	1

	.size	map_java, 59486
/* Java to managed map: END */


/* java_name_width: START */
	.section	.rodata.java_name_width,"a",%progbits
	.type	java_name_width, %object
	.p2align	2
	.global	java_name_width
java_name_width:
	.size	java_name_width, 4
	.long	90
/* java_name_width: END */
