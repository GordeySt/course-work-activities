import React from "react";
import { List, Image, Popup } from "semantic-ui-react";
import { IAttendee } from "../../../App/models/activity";

interface IProps {
  attendees: IAttendee[] | null;
}

export const ActivityListItemAttendees: React.FC<IProps> = ({ attendees }) => {
  return (
    <List horizontal>
      {attendees?.map((attendee) => (
        <List.Item key={attendee.username}>
          <Popup
            header={attendee.displayedName}
            trigger={
              <Image
                size="mini"
                circular
                src={attendee.image || "/assets/user.jpg"}
                style={{ cursor: "pointer " }}
              />
            }
          />
        </List.Item>
      ))}
    </List>
  );
};
