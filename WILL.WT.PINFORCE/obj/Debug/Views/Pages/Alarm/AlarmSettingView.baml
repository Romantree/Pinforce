   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullW0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Alarm"WILL.WT.PINFORCE.Views.Pages.Alarm  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      a5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Alarm'WILL.WT.PINFORCE.ViewModels.Pages.Alarm  5      3clr-namespace:WILL.WT.PINFORCEWILL.WT.PINFORCE  5	      9    3WILL.WT.PINFORCE.Views.Pages.Alarm.AlarmSettingView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Alarm     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5      Dvm5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Alarm     5      -emclr-namespace:WILL.WT.PINFORCE     5	         xPageSub#��[  5      .��5      �   � RowDefinitions  5   
   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      '  !TS.FW.Wpf.v2.Controls.TextBlock3D    
xTextTitle#��[ 6   $	��150��6A      Text$ SEARCH��6M   $  TS.FW.Wpf.v2.Controls.ValueBtn  5        	xValueBtn#��[ 6   $	��385��6=   ��6I   �� *Search+$  TS.FW.Wpf.v2.Controls.EventBtn  5      $	��150��6     	xEventBtn#��[ 6&   $��Search��6I     5      $	��150��6   #��[ 6&   $��Refresh��6I   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   .+�5        #��[ 6   $	��150��6A   $
 Page��6M     5        
xTextState#��[ 6   $	��150��6A    6M   �� *Page+  5      $��75��6   #��[ 6%   $��First��6H     5      $	��150��6   #��[ 6&   $
��Prev��6I     5       $	��150��6   #��[ 6&   $
��Next��6I     5!      $��75��6   #��[ 6%   $
��Last��6H   5"        5$   
   $��1��6   $	��150��6#   #��[ 6/   $��Right=�6R   $��Modify��6n     5&   
   $��2��6   #��[ 6&   $ Search Condition��6J   &   System.Windows.Controls.DataGrid  5(   
   $��3��6   ��6    �� *	AlarmList+ �� SelectedItem 6B   �� *Select+   Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*        GridTextBase1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6%   ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6R      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6^      
IsReadOnly . 	   Binding	 6|   �� *ID+  5+      # [ 6%   $ ALARM��6R   $ * 6a    . 	 6}   �� *Alarm+  5,      # [ 6%   $ LEVEL��6R   $	 150 6a    . 	 6   �� *Level+  5-      # [ 6%   $ ENABLE��6R   $	 150 6b    . 	 6�   �� *Enable+  
xStringCvt
 �� 	Converter#
 [  �� ConverterParameter$ ON/OFF��5/      � 51   
   $��4��6*   $	��600��67   $	��200��6C   $��Center=�6P   $��Slide=�6c   ��6   �� *IsOpen+.��52      �� 
������� �� CornerRadius$ 2��6,    � DataContext 6=   �� *Modfiy+._�53      � $��3q�6     54      �� 55      �� 56      �� 57      �� 58       � ColumnDefinitions 5:      �� 5;      $	��100 �6+   �� 5<      .+�5?        $��0��6%   #��[ 62   $ ALARM��6V     5@      $��1��6%   #��[ 62   $ LEVEL��6V     5A      $��2��6%   #��[ 62   $ ENABLE��6V     5C      $��0��6%   $��1��62   #��[ 6B    6f   �� *Alarm+� 5D      $��1��6   $��1��6(    � 	Resources 5E        
xRadioBase �� 
TargetType$	  TS.FW.Wpf.v2.Controls.RadioBtn �� Property �� Value  xEventBtnBackground 	 xEventBtnForeground��%]  &	       7  7 	 7 	 7 	 �� 5F      " 	 67   .�5G   "   ��  � ��$ 2q�6;   �� 5H   "    � ��$
 Bold"�6?   �� 5I   "    � ��8   6?   �� 5J   "    � ��8  6?   �� 5K   "   
 �@	 On8  67   �� 5L   "    �@	 Off8  68   �� 5M   "    � ��
 �����6@   �� 5N   "    � ��$ 1 1 0 0q�6D   5O      5P       5Q      �� 5R      �� 5S      �� 5T      �� 5U      .+�5X      	  $��0��6&   #��[ 66   $��Light��6Z    d� 	IsChecked 6j   �� *Light+	  5Y      $��1��6&   #��[ 66   $��	CycleStop��6Z    6n   �� *	CycleStop+	  5Z      $��2��6&   #��[ 66   $��Heavy��6Z    6j   �� *Heavy+	  5[      $��3��6&   #��[ 66   $��Critical��6Z    6m   �� *
Critical+5\      $
  TS.FW.Wpf.v2.Controls.OnOffBtn
  5]      $��2��6"   $��1��6/   
�������
������� 
  MarginContent$ 2q�5^   H   $
��Bold"�5_   "   $��2q�64    
  	IsChecked 5]   ?   �� *Enable+ 
  ContentColor 5_   ?   ��  �� RelativeSource ��  �� Mode$
 Self=� �� Path$ 	IsChecked� 
 	xColorCvt#
 [
 $ OnOff��� 5`      $��3��6   $��1��6(    5a      �� 5b      �� 5c      .+�5f        $��0��6&   #��[ 66   $
��SAVE��6Y   
��� � ���5g   &   ��  �� $ FindAncestor=� �� AncestorType9   3WILL.WT.PINFORCE.Views.Pages.Alarm.AlarmSettingView"  $ DataContext.OnSaveCmd���5h   &   ��   5i      $��1��6&   #��[ 66   $��CLOSE��6Y   
���  ����5j   &   ��  �� $ FindAncestor=�"  $ DataContext.OnCloseCmd���5k   &   �� 5l      5m      5n      5o      5p      5q      