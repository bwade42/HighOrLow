A = readmatrix('heartprob.csv');

%Fisher Yates
y1 = A(:,1);
%Naive Shuffle
y2 = A(:,2);
%Smart Shuffle
y3 = A(:,3);

% 20 trials
x = linspace(1,20,20);
figure
plot(x,y1)
hold on
plot(x,y2)
hold on
plot(x,y3)
hold off

xlabel('Number of trials')
ylabel('Average Number of Hearts')
title('Shuffling Algorithmn Comparison')
legend('Fisher Yates','Naive','Smart')