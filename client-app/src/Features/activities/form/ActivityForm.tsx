import React, { useEffect, useState } from "react";
import { Segment, Form, Button, Icon, Grid } from "semantic-ui-react";
import { ActivityFormValues, IActivity } from "../../../App/models/activity";
import { v4 as uuid } from "uuid";
import ActivityStore from "../../../App/stores/ActivityStore";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router-dom";
import { Form as FinalForm, Field } from "react-final-form";
import { TextInput } from "../../../App/common/form/TextInput";
import { TextAreaInput } from "../../../App/common/form/TextAreaInput";
import { SelectInput } from "../../../App/common/form/SelectInput";
import { category } from "../../../App/common/options/categoryOptions";
import { DateInput } from "../../../App/common/form/DateInput";
import {
  combineValidators,
  composeValidators,
  hasLengthGreaterThan,
  isRequired,
} from "revalidate";

const validate = combineValidators({
  title: isRequired({ message: "The title is required" }),
  category: isRequired("Category"),
  description: composeValidators(
    isRequired("Description"),
    hasLengthGreaterThan(4)({
      message: "Description needs to be at least 5 characters",
    })
  )(),
  city: isRequired("City"),
  venue: isRequired("Venue"),
  date: isRequired("Date"),
});

interface DetailsParams {
  id: string;
}

export const ActivityForm: React.FC<
  RouteComponentProps<DetailsParams>
> = observer(({ match, history }) => {
  const { loadActivity, loadingInitial } = ActivityStore;

  const [activity, setActivity] = useState(new ActivityFormValues());

  useEffect(() => {
    if (match.params.id) {
      loadActivity(match.params.id).then((activity) =>
        setActivity(new ActivityFormValues(activity))
      );
    }
  }, [loadActivity, match.params.id]);

  const handleFinalFormSubmit = (values: IActivity) => {
    if (!activity.id) {
      let newActivity = {
        ...values,
        id: uuid(),
      };
      ActivityStore.createActivity(newActivity);
    } else {
      ActivityStore.editActivity(values);
    }
  };

  const redirectFromForm = () => {
    !activity.id
      ? history.push("/activities")
      : history.push(`/activities/${activity.id}`);
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <React.Fragment>
          <Icon
            onClick={redirectFromForm}
            name="arrow alternate circle left outline"
            size="big"
            style={{ marginBottom: "10px", cursor: "pointer" }}
          />
          <Segment clearing>
            <FinalForm
              validate={validate}
              initialValues={activity}
              onSubmit={handleFinalFormSubmit}
              render={({ handleSubmit, invalid, pristine }) => (
                <Form onSubmit={handleSubmit} loading={loadingInitial}>
                  <Field
                    name="title"
                    placeholder="Title"
                    value={activity.title}
                    component={TextInput}
                  />
                  <Field
                    name="description"
                    rows={3}
                    placeholder="Description"
                    value={activity.description}
                    component={TextAreaInput}
                  />
                  <Field
                    name="category"
                    placeholder="Category"
                    options={category}
                    value={activity.category}
                    component={SelectInput}
                  />
                  <Field
                    name="date"
                    placeholder="Date"
                    value={activity.date}
                    component={DateInput}
                  />
                  <Field
                    name="city"
                    placeholder="City"
                    value={activity.city}
                    component={TextInput}
                  />
                  <Field
                    name="venue"
                    placeholder="Venue"
                    value={activity.venue}
                    component={TextInput}
                  />
                  <Button
                    disabled={loadingInitial || invalid || pristine}
                    loading={ActivityStore.submitting}
                    floated="right"
                    inverted
                    secondary
                    content="Submit"
                  />
                </Form>
              )}
            ></FinalForm>
          </Segment>
        </React.Fragment>
      </Grid.Column>
    </Grid>
  );
});
