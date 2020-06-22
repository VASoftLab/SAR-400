clc; clear all; close all;

% ������������� ����������
% ������ � ��������� ���������
test_run_data= cell(1,30);

test_run_data_selected = cell(1,30);

% ����������� ��������, ������� ��������� �����
peak_min_value = 100;

% ��������� ������
for i=1:30
    filename = sprintf('test_%d.csv',i);
    test_run_data{i} = readtable(filename,'Delimiter',';');
end

% �������� ����� ��� ��������
peak_joint_names = {'Time','R_Finger_Ring','R_Finger_Index'};

% ������ ������� ������ ��� ��������. � ������ ������ 'R_Finger_Ring','R_Finger_Index'
% ����. � ������� ������ �������������� ������� Time
for i=1:30
    test_run_data_selected{i} = test_run_data{i}(:,peak_joint_names);
end

% �������� �������������� ��������
funcion_centered = centerPeaks(test_run_data_selected, peak_min_value);

% ����� �� ����� �������� �� � ����� ������������� ��� ���������
for i = 2:size(peak_joint_names,2)
    figure
    subplot(1,2,1)
    for j=1:30
        plot(test_run_data_selected{j}.Time, test_run_data_selected{j}.(peak_joint_names{i}));
        hold on;
    end
    title(peak_joint_names{i});
    grid on

    subplot(1,2,2)
    for j=1:30
        plot(funcion_centered{j}.Time, funcion_centered{j}.(peak_joint_names{i}));
        hold on;
    end
    title([peak_joint_names{i} ' centered']);
    grid on
end
