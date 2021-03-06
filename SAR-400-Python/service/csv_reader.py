import csv
import os

from robot.robot_command import RobotCommand


def parse_csv(csv_filename, delimiter):
    with open(os.getcwd() + '/resources/' + csv_filename) as csv_file:
        robot_commands = []
        previous_command_time = 0.0
        reader = csv.DictReader(csv_file, delimiter=delimiter, quoting=csv.QUOTE_NONE)
        for row in reader:
            command_joints = []
            # todo handle situation whether Time csv header could be changed
            current_command_time = convert_string_to_number(row.get("Time"))
            command_duration = current_command_time - previous_command_time
            for row_key in row.keys():
                if row_key == "Time" or row_key == "TorsoS":
                    continue
                if row_key == "L.Finger.ThumbS":
                    command_joints.append(
                        RobotCommand.CommandJoint("L.Finger.ThumbR", convert_string_to_number(row.get(row_key))))
                    continue
                if row_key == "R.Finger.ThumbS":
                    command_joints.append(
                        RobotCommand.CommandJoint("R.Finger.ThumbR", convert_string_to_number(row.get(row_key))))
                    continue
                command_joints.append(RobotCommand.CommandJoint(row_key, convert_string_to_number(row.get(row_key))))

            robot_commands.append(RobotCommand(command_duration, command_joints))
            previous_command_time = current_command_time
    return robot_commands


def convert_string_to_number(number_as_string):
    if isinstance(number_as_string, str):
        number_correct_delimiter = number_as_string.replace(",", ".")
        number_of_dots_in_number = number_correct_delimiter.count(".")
        return float(number_correct_delimiter.replace(".", "", number_of_dots_in_number - 1))
    raise ValueError("Could not convert. Provided number as string is not a string")
