   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullS.clr-namespace:WILL.WT.PINFORCE.Controls.Motion WILL.WT.PINFORCE.Controls.Motion  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      /    )WILL.WT.PINFORCE.Controls.Motion.AxisList   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @local.clr-namespace:WILL.WT.PINFORCE.Controls.Motion     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5         xDataGridBase#��[  5	   5   &   System.Windows.Controls.DataGrid    Columns  5
      4  .System.Windows.Controls.DataGridTemplateColumn  5   
   ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6!      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6/      CellTemplate 5      �� 5      .R�5      $  TS.FW.Wpf.v2.Controls.EventBtn    	xEventBtn#��[ 6"   ��5   "   �� *Name+��5   "   �� *	IsSeleted+  	xColorCvt �� 	Converter# [  �� ConverterParameter$
 Menu����5   "   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType/   )WILL.WT.PINFORCE.Controls.Motion.AxisList"  	 �� Path$!	 DataContext.OnSelectAxisCmd���5   "   �� 5      5        5   
   $ Servo��6!   $ 60 60    5      �� 5      .R�5        #��[ 6"   $��Servo��5   @   ��5   "   �� *State.IsServoOn+  
xStringCvt# [ $ On/Off����5   "   �� *State.IsServoOn+# [ $ OnOff����5   "   �� *	OnAxisCmd+6@   5      5        5    
   $ Alarm��6!   $ 60 60    5!      �� 5"      .R�5#        #��[ 6"   $��Alarm��5&   @   ��5$   "   �� *State.IsAlarm+# [ $ 
Error/None����5%   "   �� *State.IsAlarm+# [ $ Alarm����5&   "   �� *	OnAxisCmd+6@   5'      5)      0  *System.Windows.Controls.DataGridTextColumn  5*   
   $
 Busy��6   $ 60 6+   
   
IsReadOnly
 .   GridTextBase1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6H      Binding 6u   �� *State.IsBusy+# [ $ On/Off��  5+   
   $
 Home��6   $ 60 6+   
 . # [ 6H    6u   �� *State.IsHome+# [ $ On/Off��  5,   
   $ 	(+) Limit��6   $ 60 60   
 . # [ 6M    6z   �� *State.IsPlus+# [ $ On/Off��  5-   
   $ 	(-) Limit��6   $ 60 60   
 . # [ 6M    6z   �� *State.IsMinus+# [ $ On/Off��  5.   
   $ Act Position (mm)��6   $ * 68   
 . # [ 6T    6�   �� *State.ActPos+ �� StringFormat$ {0:f3}��  5/   
   $ Cmd Position (mm)��6   $ * 68   
 . # [ 6T    6�   �� *State.CmdPos+$ {0:f3}��  50   
   $  ��6!   $ 60 6+    51      �� 52      .R�53        #��[ 6"   $��Reset��54   "   $��Reset��55   @   ��6"   �� *	OnAxisCmd+6@   56      58      5:      