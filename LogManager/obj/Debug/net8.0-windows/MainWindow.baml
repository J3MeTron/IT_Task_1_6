   M S B A M L   `   `   `  ����E  ALogManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:LogManager
LogManager  5   	       LogManager.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:LogManager     5   	     � Title$  Лог-менеджер��5   	   $	��450��6   $	��800��6+   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5      �� $	��120��6   $��0,0,10,0q�6#   ��5      ��    MessageTypes �� Source# [   �� SelectedItem 5      �� *SelectedMessageType+�� 5      $	��400��6   $��0,0,10,0q�6"   ��5      �� *
NewMessage+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      $��Добавить��6   ��6(   �� *AddMessageCommand+5      �� 5   
   $��1��6   $ 
Horizontal=�6"   $��0,0,0,10q�6;   .+�5      �� $ ��Фильтр по типу��6   $��0,0,10,0q�6V   ��6.   �� *FilterByTypeCommand+6V   �� 5      $��От:��6   $��Center=�6#   $��0,0,5,0q�6>   (  "System.Windows.Controls.DatePicker  5      $��0,0,10,0q�6<      SelectedDate 6   �� *	StartDate+6<   "   LogManager.TimeMaskedTextBox  5       $��60��6&   $��0,0,10,0q�6q      Time 61   �� *	StartTime+$ PropertyChanged=�6q   �� 5!      $��До:��6   $��Center=�6#   $��0,0,5,0q�6>     5"      $��0,0,10,0q�6:    6   �� *	EndDate+6:     5#      $��60��6&   $��0,0,10,0q�6o    61   �� *	EndTime+$ PropertyChanged=�6o   �� 5$      $ ��Фильтр по дате��6   $��0,0,10,0q�6[   ��6.   �� *FilterByDateRangeCommand+6[   �� 5%      $%��Сбросить фильтры��6   ��60   �� *ResetFiltersCommand+5&      �� 5)   
   $��2��6   $��0,0,0,10q�6A   ��6    �� *
Messages+ �� View 5*      �� 5+      .P�5,      �� $��Тип��6%   	 �� Width$		 100��62   
 �� DisplayMemberBinding
 6>   �� *Type+ �� HeaderTemplate 5-      �� 5.      .R�5/   "   ]� .+�50   &   �� -   5        $��Тип��50   0   6;   F� 51   &    F�Name$ UpArrow��6+   $y�	Collapsed=�6<   
���   �$��1��6b   "����@KL ��� р�� @KL ���� ��� 	6v   F� 52   &   $ 	DownArrow��6+   $y�	Collapsed=�6>   
���   �$��1��6d   "����@KL @KL р�� ��� ���� @KL 	6x   53   #   54      56      �� 57      $��Дата и время��6%   $		 150��6;   
 6G   �� *	Timestamp+ 58      �� 59      .R�5:   "   ]� .+�5;   &   �� -   5        $��Дата и время��5;   0   6D   F� 5<   &   $ UpArrow��6+   $y�	Collapsed=�6<   
���   �$��1��6b   "����@KL ��� р�� @KL ���� ��� 	6v   F� 5=   &   $ 	DownArrow��6+   $y�	Collapsed=�6>   
���   �$��1��6d   "����@KL @KL р�� ��� ���� @KL 	6x   5>   #   5?      5A      �� 5B      $��Сообщение��6%   $		 400��68   
 6D   �� *	Message+ 5C      �� 5D      .R�5E   "   ]� .+�5F   &   �� -   5        $��Сообщение��5F   0   6A   F� 5G   &   $ UpArrow��6+   $y�	Collapsed=�6<   
���   �$��1��6b   "����@KL ��� р�� @KL ���� ��� 	6v   F� 5H   &   $ 	DownArrow��6+   $y�	Collapsed=�6>   
���   �$��1��6d   "����@KL @KL р�� ��� ���� @KL 	6x   5I   #   5J      5L      5M      5O      � 5R   
   $��3��6    � ColumnDefinitions 5S      �� 5T      $��* �6#   �� 5U      $
��Auto �6#   .+�5W      �� $��Center=�5X      ��5W      �� *MessageCount+ �� StringFormat$( "Всего сообщений: {0}��5X      �� 5Y      $��1��6   $$��Сохранить в файл��6%   ��6@   �� *SaveToFileCommand+5Z      5[      5\      