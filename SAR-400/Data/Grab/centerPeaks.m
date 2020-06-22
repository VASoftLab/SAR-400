function out = centerPeaks(data,peak_min_value)

    % ����������� ���������� ��������� ������
    [~,tableCount]=size(data);
    [~,columnCount] = size(data{1});
    
    % ���������� �������� ��� ������������ ����� ������ ����� �������� �
    % ������� ����� 1 (�.�. ��������� ������� �������)
    columnCount = columnCount - 1;
    
    for c = 1:columnCount
        % ������������� ����������
        avg_peak = zeros(1,tableCount);
        
        % ��������� �� ������� ������� ��� ������������
        currentColumn = 1 + c;

        % �������� ������ �������� ���� ��� ������ �������
        for i=1:tableCount
            peakrows = find(data{i}{:,currentColumn} > peak_min_value);
            avg_peak(i) = round(mean(peakrows));
        end
        
        % �������� ������ ����� �������� ����
        peak_center = round(mean(avg_peak));

        % ������� ������ �� �������� ��� ���������� �� � ����� ��������
        for i=1:tableCount
            % ������� �������� � ��������
            diff = peak_center - avg_peak(i);
            
            
            % �������� �� ��������� - ��������� � ��������� �������
            if diff == 0
                continue;
            end
            
            % ���� ����� ���� ��� ������ ������� ������, ��� ������ �����
            % ��������
            if diff > 0
                % ������� ������� � ����� ������� �� diff ��������
                data{i}{(1+diff):end,currentColumn} = data{i}{1:(end-diff),currentColumn};
                
                % ��������� �������������� ������� � ������ ������� ������
                % ������
                for j = 1:diff
                    data{i}{j,currentColumn} = data{i}{1+diff,currentColumn};
                end
            end
            
            if diff < 0
                % �������� ���������� �������� ������� (���������� ���
                % ����, ����� �������� �������� � ������)
                diff = abs(diff);
                
                % ������� ������� � ������ ������� �� diff ��������
                data{i}{1:(end-diff),currentColumn} = data{i}{(1+diff):end,currentColumn};
                
                % ��������� �������������� ������� � ����� �������
                % ��������� ������
                for j = 1:diff
                    data{i}{(end - diff + j),currentColumn} = data{i}{(end-diff),currentColumn};
                end
            end
        end
    end
    
    % ���������� ���������
    out = data;
end