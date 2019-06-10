class RobotCommand:

    def __init__(self, command_duration, command_joints=[]):
        self.command_duration = command_duration
        self.command_joints = command_joints

    class CommandJoint:

        def __init__(self, joint_name, joint_value):
            self.joint_name = joint_name
            self.joint_value = joint_value
